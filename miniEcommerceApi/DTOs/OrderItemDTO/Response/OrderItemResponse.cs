namespace miniEcommerceApi.DTOs.OrderItemDTO.Response
{
    public record OrderItemResponse
    {
        public Guid Id { get; init; }
        public Guid ProductId { get; init; }
        public string ProductName { get; init; }
        public int Quantity { get; init; }
        public decimal UnitPrice { get; init; }
        public decimal SubTotal => UnitPrice * Quantity;
    }
}