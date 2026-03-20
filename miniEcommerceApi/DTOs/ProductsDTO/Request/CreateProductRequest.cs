using System.ComponentModel.DataAnnotations;

namespace miniEcommerceApi.DTOs.ProductDTO.Request
{
    public class CreateProductRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name must have at most 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500, ErrorMessage = "Description must have at most 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than or equal to zero")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [Url(ErrorMessage = "Invalid image URL")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "CategoryId is required")]
        public Guid CategoryId { get; set; }
    }
}
