    namespace miniEcommerceApi.Models
    {
        public class OrderItem
        {
            public Guid Id { get; set; }
            public Guid OrderId { get; set; }
            public Guid ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal SubTotal => UnitPrice * Quantity;
            public Orders Order { get; set; } = null!;
            public Products Product { get; set; } = null!;
        }
    }
