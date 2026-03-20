using miniEcommerceApi.DTOs.OrderItemDTO.Response;
using miniEcommerceApi.DTOs.OrdersDTO.Response;
using miniEcommerceApi.Models;

namespace miniEcommerceApi.Mappings
{
    public static class OrderMappingExtensions
    {
        public static OrderResponse ToResponse(this Orders order) => new OrderResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            OrderDate = order.OrderDate,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            ZipCode = order.ZipCode,
            Street = order.Street,
            Number = order.Number,
            Neighborhood = order.Neighborhood,
            City = order.City,
            State = order.State,
            Notes = order.Notes,
            Items = order.Items.Select(i => i.ToResponse()).ToList()
        };
        public static OrderItemResponse ToResponse(this OrderItem item) => new OrderItemResponse
        {
            Id = item.Id,
            ProductId = item.ProductId,
            ProductName = item.Product.Name,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice
        };
    }
}
