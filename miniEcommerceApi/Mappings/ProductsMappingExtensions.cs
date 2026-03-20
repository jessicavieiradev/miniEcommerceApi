using miniEcommerceApi.DTOs.ProductDTO.Response;
using miniEcommerceApi.Models;

namespace miniEcommerceApi.Mappings
{
    public static class ProductsMappingExtensions
    {
        public static ProductResponse ToResponse(this Products product) => new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId,
            CategoryName = product.Category.Name,
            CreatedAt = product.CreatedAt
        };
    }
}
