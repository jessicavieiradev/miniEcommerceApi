using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using miniEcommerceApi.Data;
using miniEcommerceApi.Enums;
using miniEcommerceApi.Interfaces;
using miniEcommerceApi.Models;
using miniEcommerceApi.Services;
using miniEcommerceApi.Tests.Builders;
using static miniEcommerceApi.Tests.Helpers.PaymentApproval;

namespace miniEcommerceApi.Tests
{
    public class PaymentServiceTests
    {
        private AppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }
        private PaymentService CreateService(AppDbContext context, bool approved = true)
        {
            IRandomPaymentApproval approval = approved ? new AlwaysApproved() : new AlwaysRefused();
            return new PaymentService(context, approval);
        }

        //GetPaymentByOrderId
        //- payment found, return payment response
        [Fact]
        public async Task GetPaymentByOrderId_PaymentExists_ReturnsPaymentResponse()
        {
            //arrange
            var context = GetInMemoryContext();
            var orderId = Guid.NewGuid();

            var payment = new Payments(orderId, Enums.PaymentMethod.Pix, Enums.PaymentStatus.Approved, 100);
            context.Payments.Add(payment);
            await context.SaveChangesAsync();

            var service = CreateService(context);

            //act
            var result = await service.GetPaymentByOrderId(orderId);

            //assert
            result.Should().NotBeNull();
            result.OrderId.Should().Be(orderId);
        }
        //- payment not found, returns keynotfoundexception
        [Fact]
        public async Task GetPaymentByOrderId_PaymentDontExists_ReturnsKeyNotFoundException()
        {
            //arrange
            var context = GetInMemoryContext();
            var service = CreateService(context);

            //act & assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetPaymentByOrderId(Guid.NewGuid()));
            exception.Message.Should().Be("Payment not found");
        }

        //ProcessPayment
        //- order not found, returns KeyNotFoundException
        [Fact]
        public async Task ProcessPayment_OrderDontExists_ReturnsKeyNotFoundException()
        {
            //arrange
            var context = GetInMemoryContext();
            var service = CreateService(context);

            var dto = PaymentRequestBuilder.WithInvalidOrder();

            //act & assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => service.ProcessPayment(dto)
            );
            exception.Message.Should().Be("Order not found");
        }

        //- order is not pending, returns InvalidOperationException
        [Theory]
        [InlineData(OrderStatus.Confirmed)]
        [InlineData(OrderStatus.Shipped)]
        [InlineData(OrderStatus.Delivered)]
        [InlineData(OrderStatus.Cancelled)]
        public async Task ProcessPayment_OrderIsNotPending_ReturnsInvalidOperationException(OrderStatus status)
        {
            //arrange
            var context = GetInMemoryContext();
            var service = CreateService(context);

            var order = OrderBuilder.Default();
            order.Status = status;
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var dto = PaymentRequestBuilder.Default(order.Id);

            //act & assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.ProcessPayment(dto)
            );
            exception.Message.Should().Be("Order is not pending");
        }

        //-change order status to preparing
        [Fact]
        public async Task ProcessPayment_ValidRequest_ChangesOrderStatusToPreparing()
        {
            //arrange
            var context = GetInMemoryContext();
            var service = CreateService(context);
            var order = OrderBuilder.Default();
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            var dto = PaymentRequestBuilder.Default(order.Id);
            //act
            await service.ProcessPayment(dto);
            //assert
            var updatedOrder = await context.Orders.FindAsync(order.Id);
            updatedOrder.Should().NotBeNull();
            updatedOrder.Status.Should().Be(OrderStatus.Preparing);
        }

        //-check if stock is reduced when payment is approved
        [Fact]
        public async Task ProcessPayment_ApprovedPayment_ReducesStock()
        {
            //arrange
            var context = GetInMemoryContext();
            var service = CreateService(context);
            var order = OrderBuilder.Default();
            var product = ProductBuilder.Default();
            var item = OrderItemBuilder.Default(order.Id, product.Id);
            context.Products.Add(product);
            context.Orders.Add(order);
            context.OrderItem.Add(item);
            await context.SaveChangesAsync();
            var expectedStock = product.Stock - item.Quantity;
            var dto = PaymentRequestBuilder.Default(order.Id);

            //act
            await service.ProcessPayment(dto);

            //assert
            var updatedProduct = await context.Products.FindAsync(product.Id);
            updatedProduct.Should().NotBeNull();
            updatedProduct.Stock.Should().Be(expectedStock);
        }

        //- check if payment is approved when approval returns true
        [Fact]
        public async Task ProcessPayment_ApprovalReturnsTrue_SetsPaymentStatusToApproved()
        {
            //arrange
            var context = GetInMemoryContext();
            var service = CreateService(context);
            var order = OrderBuilder.Default();
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            var dto = PaymentRequestBuilder.Default(order.Id);

            //act
            await service.ProcessPayment(dto);

            //assert
            var payment = await context.Payments.FirstOrDefaultAsync(p => p.OrderId == order.Id);
            payment.Should().NotBeNull();
            payment.Status.Should().Be(PaymentStatus.Approved);
        }

        // - check if payment is refused when approval returns false
        [Fact]
        public async Task ProcessPayment_ApprovalReturnsFalse_SetsPaymentStatusToRefused()
        {
            //arrange
            var context = GetInMemoryContext();
            var service = CreateService(context, approved: false);
            var order = OrderBuilder.Default();
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            var dto = PaymentRequestBuilder.Default(order.Id);
            //act
            await service.ProcessPayment(dto);
            //assert
            var payment = await context.Payments.FirstOrDefaultAsync(p => p.OrderId == order.Id);
            payment.Should().NotBeNull();
            payment.Status.Should().Be(PaymentStatus.Refused);
        }

        // - check if stock is not reduced when payment is refused
        [Fact]
        public async Task ProcessPayment_RefusedPayment_DoesNotReduceStock()
        {
            //arrange
            var context = GetInMemoryContext();
            var service = CreateService(context, approved: false);
            var order = OrderBuilder.Default();
            var product = ProductBuilder.Default();
            var item = OrderItemBuilder.Default(order.Id, product.Id);
            context.Products.Add(product);
            context.Orders.Add(order);
            context.OrderItem.Add(item);
            await context.SaveChangesAsync();
            var expectedStock = product.Stock;
            var dto = PaymentRequestBuilder.Default(order.Id);
            //act
            await service.ProcessPayment(dto);
            //assert
            var updatedProduct = await context.Products.FindAsync(product.Id);
            updatedProduct.Should().NotBeNull();
            updatedProduct.Stock.Should().Be(expectedStock);
        }

        // - check if the order was set cancelled when payment is refused
        [Fact]
        public async Task ProcessPayment_RefusedPayment_SetsOrderStatusCancelled()
        {
            //arrange
            var context = GetInMemoryContext();
            var service = CreateService(context, approved: false);
            var order = OrderBuilder.Default();
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            var dto = PaymentRequestBuilder.Default(order.Id);
            //act
            await service.ProcessPayment(dto);
            //assert
            var updatedOrder = await context.Orders.FindAsync(order.Id);
            updatedOrder.Should().NotBeNull();
            updatedOrder.Status.Should().Be(OrderStatus.Cancelled);
        }

        // - stock insufficient, returns invalidoperationexception
        [Fact]
        public async Task ProcessPayment_StockInsufficient_ReturnsInvalidOperationException()
        {
            //arrange
            var context = GetInMemoryContext();
            var service = CreateService(context);
            var order = OrderBuilder.Default();
            var product = ProductBuilder.Default();
            product.Stock = 0;
            var item = OrderItemBuilder.Default(order.Id, product.Id);
            context.Products.Add(product);
            context.Orders.Add(order);
            context.OrderItem.Add(item);
            await context.SaveChangesAsync();
            var dto = PaymentRequestBuilder.Default(order.Id);
            //act & assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.ProcessPayment(dto)
            );
            exception.Message.Should().Be($"Insufficient stock for product {item.Product.Name}");
        }

        // - stock is exactly the quantity, payment is approved, reduces stock to 0
        [Fact]
        public async Task ProcessPayment_StockEqualsQuantity_ReducesStockToZero()
        {
            //arrange
            var context = GetInMemoryContext();
            var service = CreateService(context);
            var order = OrderBuilder.Default();
            var product = ProductBuilder.Default();
            product.Stock = 5;
            var item = OrderItemBuilder.Default(order.Id, product.Id, quantity: 5);
            context.Products.Add(product);
            context.Orders.Add(order);
            context.OrderItem.Add(item);
            await context.SaveChangesAsync();
            var dto = PaymentRequestBuilder.Default(order.Id);
            //act
            await service.ProcessPayment(dto);
            //assert
            var updatedProduct = await context.Products.FindAsync(product.Id);
            updatedProduct.Should().NotBeNull();
            updatedProduct.Stock.Should().Be(0);
        }
    }
}
