using miniEcommerceApi.DTOs.CategoriesDTO.Request;
using miniEcommerceApi.DTOs.CategoriesDTO.Response;

namespace miniEcommerceApi.Interfaces
{
    public interface ICategoriesService
    {
        public Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync();
        public Task<CategoryResponse> GetCategoryByIdAsync(Guid id);
        public Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest dto);
        public Task<CategoryResponse> UpdateCategoryAsync(Guid id,UpdateCategoryRequest dto);
        public Task DeleteCategoryAsync(Guid id);
    }
}
