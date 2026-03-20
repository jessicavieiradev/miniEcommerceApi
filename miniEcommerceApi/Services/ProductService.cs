using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using miniEcommerceApi.Data;
using miniEcommerceApi.DTOs.OrderItemDTO.Response;
using miniEcommerceApi.DTOs.ProductDTO.Request;
using miniEcommerceApi.DTOs.ProductDTO.Response;
using miniEcommerceApi.Interfaces;
using miniEcommerceApi.Mappings;
using miniEcommerceApi.Models;

namespace miniEcommerceApi.Services
{
    public class ProductService : IProductsService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductResponse>> GetAllProducts()
        {
            var products = await _context.Products.Include(c => c.Category).Where(p => p.IsActive).ToListAsync();

            if (!products.Any())
                return Enumerable.Empty<ProductResponse>();

            return products.Select(p => p.ToResponse());
        }
        public async Task<IEnumerable<ProductResponse>> GetAllProductsAdmin()
        {
            var products = await _context.Products.Include(c => c.Category).ToListAsync();

            if (!products.Any())
                return Enumerable.Empty<ProductResponse>();

            return products.Select(p => p.ToResponse());
        }

        public async Task<ProductResponse> GetProductById(Guid id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (product == null)
                throw new KeyNotFoundException("Product not found");

            return product.ToResponse();
        }

        public async Task<IEnumerable<ProductResponse>> GetProductsByCategory(Guid categoryId)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(c => c.CategoryId == categoryId && c.IsActive)
                .ToListAsync();

            if (!products.Any())
                return Enumerable.Empty<ProductResponse>();

            return products.Select(p => p.ToResponse());
        }

        public async Task<ProductResponse> CreateProduct(CreateProductRequest dto)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == dto.CategoryId);

            if (category == null)
                throw new KeyNotFoundException("Category not found");

            var product = new Products(
                dto.Name,
                dto.Description,
                dto.Price,
                dto.Stock,
                dto.ImageUrl,
                dto.CategoryId,
                true
            );

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return await GetProductById(product.Id);
        }
        public async Task<ProductResponse> UpdateProduct(Guid id, UpdateProductRequest dto)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                throw new KeyNotFoundException("Product not found");

            if (dto.Name != null) product.Name = dto.Name;
            if (dto.Description != null) product.Description = dto.Description;
            if (dto.Price != null) product.Price = dto.Price.Value;
            if (dto.Stock != null) product.Stock = dto.Stock.Value;
            if (dto.ImageUrl != null) product.ImageUrl = dto.ImageUrl;
            if (dto.CategoryId != null)
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == dto.CategoryId);

                if (category == null)
                    throw new KeyNotFoundException("Category not found");

                product.CategoryId = dto.CategoryId.Value;
            }

            await _context.SaveChangesAsync();

            return product.ToResponse();
        }
        public async Task ToggleProductStatus(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                throw new KeyNotFoundException("Product not found");

            product.IsActive = !product.IsActive;
            await _context.SaveChangesAsync();
        }
    }
}
