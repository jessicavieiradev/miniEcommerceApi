using miniEcommerceApi.Enums;

namespace miniEcommerceApi.Models
{
    public class Payments
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Orders Order { get; set; } = null!;
        public Payments() { }

        public Payments(Guid orderId, PaymentMethod method, PaymentStatus status, decimal amount)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            Method = method;
            Status = status;
            Amount = amount;
            CreatedAt = DateTime.UtcNow;
        }

    }
}
