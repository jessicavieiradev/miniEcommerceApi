using Microsoft.EntityFrameworkCore;
using miniEcommerceApi.Models;

namespace miniEcommerceApi.Data.Seeders
{
    public class ProductsSeeder
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public ProductsSeeder(AppDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task SeedAsync()
        {
            try
            {
                if (await _context.Products.AnyAsync()) return;

                var response = await _httpClient.GetFromJsonAsync<DummyResponse>("https://dummyjson.com/products");

                if (response?.Products == null)
                    throw new Exception("Dados de produtos ausentes na resposta.");

                foreach (var item in response.Products)
                {
                    var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == item.Category);
                    if (category == null)
                    {
                        category = new Categories(item.Category);
                        _context.Categories.Add(category);
                        await _context.SaveChangesAsync();
                    }
                    var primeiraImagem = item.Images.FirstOrDefault() ?? "sem-imagem.jpg";
                    var product = new Products(item.Title, item.Description, (decimal)item.Price, item.Stock, primeiraImagem, category.Id, true);
                    _context.Products.Add(product);
                }
                await _context.SaveChangesAsync();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("O serviço de produtos está indisponível.", ex);
            }
        }

        public record DummyResponse(List<DummyProduct> Products);
        public record DummyProduct(string Title, string Description, double Price,int Stock, List<string> Images, string Category);
    }
}
