using miniEcommerceApi.DTOs.OrderItemDTO.Request;
using System.ComponentModel.DataAnnotations;

namespace miniEcommerceApi.DTOs.OrdersDTO.Request
{
    public record CreateOrderRequest
    {

        [Required(ErrorMessage = "CustomerId is required")]
        public Guid CustomerId { get; init; }
        [Required(ErrorMessage = "AddressId is required")]
        public Guid AddressId { get; set; }
        public string? Notes { get; init; }

        [Required(ErrorMessage = "Items are required")]
        [MinLength(1, ErrorMessage = "Order must have at least one item")]
        public List<CreateOrderItemRequest> Items { get; init; }
        public CreateOrderRequest() { }

        public CreateOrderRequest(
            Guid customerId,
            Guid AddressId,
            string? notes,
            List<CreateOrderItemRequest> items)
        {
            CustomerId = customerId;
            AddressId = AddressId;
            Notes = notes;
            Items = items;
        }
    }
}