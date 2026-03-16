using Microsoft.EntityFrameworkCore;
using miniEcommerceApi.Data;
using miniEcommerceApi.Data.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine(">>> STRING DE CONEXÃO CARREGADA: " + connectionString);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();

builder.Services.AddHttpClient();

builder.Services.AddScoped<ProductsSeeder>();

var app = builder.Build();

// Configure the HTTP request pipeline.

using(var scope = app.Services.CreateScope())
{    
    var seeder = scope.ServiceProvider.GetRequiredService<ProductsSeeder>();
    await seeder.SeedAsync();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
