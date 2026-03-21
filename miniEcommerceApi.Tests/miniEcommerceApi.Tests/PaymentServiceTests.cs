using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using miniEcommerceApi.Data;
using miniEcommerceApi.DTOs.PaymentDTO.Request;
using miniEcommerceApi.Enums;
using miniEcommerceApi.Models;
using miniEcommerceApi.Services;

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

            var service = new PaymentService(context);

            //act
            var result = await service.GetPaymentByOrderId(orderId);

            //assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.OrderId);
        }
        //- payment not found, returns keynotfoundexception
        [Fact]
        public async Task GetPaymentByOrderId_PaymentDontExists_ReturnsKeyNotFoundException()
        {
            //arrange
            var context = GetInMemoryContext();
            var service = new PaymentService(context);

            //act & assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetPaymentByOrderId(Guid.NewGuid()));
            Assert.Equal("Payment not found", exception.Message);
        }

        //ProcessPayment
        //- order not found, returns KeyNotFoundException
        [Fact]
        public async Task ProcessPayment_OrderDontExists_ReturnsKeyNotFoundException()
        {
            //arrange
            var context = GetInMemoryContext();
            var service = new PaymentService(context);

            var dto = new CreatePaymentRequest
            {
                OrderId = Guid.NewGuid(),
                Method = Enums.PaymentMethod.Pix
            };

            //act & assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => service.ProcessPayment(dto)
            );
            Assert.Equal("Order not found", exception.Message);
        }

        //- order is not pending, returns InvalidOperationException
        [Theory]
        [InlineData(OrderStatus.Confirmed)]
        [InlineData(OrderStatus.Shipped)]
        [InlineData(OrderStatus.Delivered)]
        [InlineData(OrderStatus.Cancelled)]
        public async Task ProcessPayment_OrderIsNotPending_ReturnsInvalidOperationException(OrderStatus status)
        {
            var context = GetInMemoryContext();
            var service = new PaymentService(context);

            var order = OrderBuilder.Default();
            order.Status = status;
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var dto = new CreatePaymentRequest
            {
                OrderId = order.Id,
                Method = Enums.PaymentMethod.Pix
            };

            //act & assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.ProcessPayment(dto)
            );
            Assert.Equal("Order is not pending", exception.Message);
        }
    }
}
