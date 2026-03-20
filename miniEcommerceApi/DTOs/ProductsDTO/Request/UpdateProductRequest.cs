using System.ComponentModel.DataAnnotations;

namespace miniEcommerceApi.DTOs.ProductDTO.Request
{
    public class UpdateProductRequest
    {
        [MaxLength(100, ErrorMessage = "Name must have at most 100 characters")]
        public string? Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description must have at most 500 characters")]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal? Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than or equal to zero")]
        public int? Stock { get; set; }

        [Url(ErrorMessage = "Invalid image URL")]
        public string? ImageUrl { get; set; }

        public Guid? CategoryId { get; set; }
    }
}
