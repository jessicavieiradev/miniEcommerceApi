using miniEcommerceApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace miniEcommerceApi.DTOs.OrdersDTO.Request
{
    public record UpdateOrderStatusRequest
    {
        [Required(ErrorMessage = "Status is required")]
        [EnumDataType(typeof(OrderStatus), ErrorMessage = "Invalid status")]
        public OrderStatus Status { get; init; }
        public UpdateOrderStatusRequest() { }

        public UpdateOrderStatusRequest(OrderStatus status)
        {
            Status = status;
        }
    }
}