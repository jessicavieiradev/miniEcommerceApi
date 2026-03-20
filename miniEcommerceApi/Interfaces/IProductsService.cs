using Microsoft.AspNetCore.Mvc;
using miniEcommerceApi.DTOs.ProductDTO.Request;
using miniEcommerceApi.DTOs.ProductDTO.Response;

namespace miniEcommerceApi.Interfaces
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductResponse>> GetAllProducts();
        Task<IEnumerable<ProductResponse>> GetAllProductsAdmin();
        Task<IEnumerable<ProductResponse>> GetProductsByCategory(Guid categoryId);
        Task<ProductResponse> GetProductById(Guid id);
        Task<ProductResponse> CreateProduct(CreateProductRequest dto);
        Task<ProductResponse> UpdateProduct(Guid id, UpdateProductRequest dto);
        Task ToggleProductStatus(Guid id);
    }
}
