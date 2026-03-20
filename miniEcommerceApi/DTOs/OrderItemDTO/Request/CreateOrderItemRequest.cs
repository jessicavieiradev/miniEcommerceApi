using System.ComponentModel.DataAnnotations;

namespace miniEcommerceApi.DTOs.OrderItemDTO.Request
{
    public record CreateOrderItemRequest
    {

        [Required(ErrorMessage = "ProductId is required")]
        public Guid ProductId { get; init; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero")]
        public int Quantity { get; init; }
        public CreateOrderItemRequest() { }

        public CreateOrderItemRequest(Guid productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }
    }
}