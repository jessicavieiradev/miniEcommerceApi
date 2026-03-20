using miniEcommerceApi.DTOs.OrderItemDTO.Response;
using miniEcommerceApi.Enums;

namespace miniEcommerceApi.DTOs.OrdersDTO.Response
{
    public record OrderResponse
    {
        public Guid Id { get; init; }
        public Guid CustomerId { get; init; }
        public DateTime OrderDate { get; init; }
        public OrderStatus Status { get; init; }
        public decimal TotalAmount { get; init; }
        public string ZipCode { get; init; }
        public string Street { get; init; }
        public string Number { get; init; }
        public string Neighborhood { get; init; }
        public string City { get; init; }
        public string State { get; init; }
        public string? Notes { get; init; }
        public List<OrderItemResponse> Items { get; init; }
    }
}