using Microsoft.EntityFrameworkCore;
using miniEcommerceApi.Data;
using miniEcommerceApi.DTOs.OrdersDTO.Request;
using miniEcommerceApi.DTOs.OrdersDTO.Response;
using miniEcommerceApi.Enums;
using miniEcommerceApi.Interfaces;
using miniEcommerceApi.Mappings;
using miniEcommerceApi.Models;

namespace miniEcommerceApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrders()
        {
            var orders = await _context.Orders.Include(o => o.Items).ThenInclude(i => i.Product).ToListAsync();

            if (!orders.Any())
                return Enumerable.Empty<OrderResponse>();

            return orders.Select(o => o.ToResponse());
        }

        public async Task<OrderResponse> GetOrderById(Guid id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                throw new KeyNotFoundException("Order not found");

            return order.ToResponse();
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersByCustomerId(Guid customerId)
        {
            var orders = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();

            if (!orders.Any())
                return Enumerable.Empty<OrderResponse>();

            return orders.Select(o => o.ToResponse());
        }

        public async Task<OrderResponse> CreateOrder(CreateOrderRequest dto)
        {
            var Customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == dto.CustomerId);

            if (Customer == null)
                throw new KeyNotFoundException("Customer not found");

            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == dto.AddressId && a.CustomerId == Customer.Id);

            if (address == null)
                throw new KeyNotFoundException("Address not found");

            var order = new Orders
            {
                CustomerId = dto.CustomerId,
                Status = OrderStatus.Pending,
                OrderDate = DateTime.UtcNow,
                ZipCode = address.ZipCode,
                Street = address.Street,
                Number = address.Number,
                Neighborhood = address.Neighborhood,
                City = address.City,
                State = address.State,
                Notes = dto.Notes
            };

            var productIds = dto.Items.Select(i => i.ProductId).ToList();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            var orderItems = new List<OrderItem>();

            foreach (var item in dto.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);

                if (product == null)
                    throw new KeyNotFoundException($"Product {item.ProductId} not found");

                orderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });
            }

            order.TotalAmount = orderItems.Sum(i => i.SubTotal);
            order.Items = orderItems;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return await GetOrderById(order.Id);
        }

        public async Task UpdateOrderStatus(Guid id, UpdateOrderStatusRequest dto)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                throw new KeyNotFoundException("Order not found");

            order.Status = dto.Status;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(Guid id)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                throw new KeyNotFoundException("Order not found");

            order.Status = OrderStatus.Cancelled;

            await _context.SaveChangesAsync();
        }
    }
}
