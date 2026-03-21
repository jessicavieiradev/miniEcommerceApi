using Microsoft.EntityFrameworkCore;
using miniEcommerceApi.Data;
using miniEcommerceApi.DTOs.PaymentDTO.Request;
using miniEcommerceApi.DTOs.PaymentDTO.Response;
using miniEcommerceApi.Enums;
using miniEcommerceApi.Interfaces;
using miniEcommerceApi.Models;

namespace miniEcommerceApi.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PaymentResponse> GetPaymentByOrderId(Guid orderId)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);

            if (payment == null)
                throw new KeyNotFoundException("Payment not found");

            return new PaymentResponse
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Method = payment.Method,
                Status = payment.Status,
                Amount = payment.Amount,
                CreatedAt = payment.CreatedAt
            };
        }

        public async Task<PaymentResponse> ProcessPayment(CreatePaymentRequest dto)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == dto.OrderId);

            if (order == null)
                throw new KeyNotFoundException("Order not found");

            if (order.Status != OrderStatus.Pending)
                throw new InvalidOperationException("Order is not pending");

            var approved = new Random().Next(0, 10) > 1;

            var payment = new Payments(
                dto.OrderId,
                dto.Method,
                approved ? PaymentStatus.Approved : PaymentStatus.Refused,
                order.TotalAmount
            );

            if (approved)
            {
                order.Status = OrderStatus.Preparing;

                var orderItems = await _context.OrderItem
                    .Include(i => i.Product)
                    .Where(i => i.OrderId == dto.OrderId)
                    .ToListAsync();

                foreach (var item in orderItems)
                {
                    if (item.Product.Stock < item.Quantity)
                        throw new InvalidOperationException($"Insufficient stock for product {item.Product.Name}");

                    item.Product.Stock -= item.Quantity;
                }
            }
            else
            {
                order.Status = OrderStatus.Cancelled;
            }

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return new PaymentResponse
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Method = payment.Method,
                Status = payment.Status,
                Amount = payment.Amount,
                CreatedAt = payment.CreatedAt
            };
        }
    }
}
