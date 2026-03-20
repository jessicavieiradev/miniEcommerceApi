using Microsoft.EntityFrameworkCore;
using miniEcommerceApi.Data;
using miniEcommerceApi.DTOs.CategoriesDTO.Request;
using miniEcommerceApi.DTOs.CategoriesDTO.Response;
using miniEcommerceApi.Interfaces;
using miniEcommerceApi.Models;

namespace miniEcommerceApi.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly AppDbContext _context;
        public CategoriesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();



            return categories.Select(c => new CategoryResponse(c.Id, c.Name, c.IsActive));
        }
        public async Task<CategoryResponse> GetCategoryByIdAsync(Guid id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            if (category == null)
            {
                throw new KeyNotFoundException("Categoria não encontrada.");
            }
            return new CategoryResponse(category.Id, category.Name, category.IsActive);
        }
        public async Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("O Name da categoria não pode ser vazio.");
            }
            var existingCategory = await _context.Categories.AnyAsync(c => c.Name == dto.Name);
            if (existingCategory)
            {
                throw new InvalidOperationException("Uma categoria com este Name já existe.");
            }

            var category = new Categories(dto.Name);

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryResponse(category.Id,category.Name,category.IsActive);
        }
        public async  Task<CategoryResponse> UpdateCategoryAsync(Guid id, UpdateCategoryRequest dto)
        {
            var existingCategory = await _context.Categories.FindAsync(id);
            if(existingCategory == null)
            {
                throw new KeyNotFoundException("Categoria não encontrada.");
            }
            existingCategory.UpdateName(dto.Name);

            await _context.SaveChangesAsync();

            return new CategoryResponse(existingCategory.Id, existingCategory.Name, existingCategory.IsActive);
        }
        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Categoria não encontrada.");
            }
            category.Deactivate();

            await _context.SaveChangesAsync();
        }
    }
}
