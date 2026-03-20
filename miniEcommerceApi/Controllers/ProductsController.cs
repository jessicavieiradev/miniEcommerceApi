using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miniEcommerceApi.DTOs.ProductDTO.Request;
using miniEcommerceApi.DTOs.ProductDTO.Response;
using miniEcommerceApi.DTOs.Shared;
using miniEcommerceApi.Enums;
using miniEcommerceApi.Interfaces;

namespace miniEcommerceApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductsController : ControllerBase
	{
		private readonly IProductsService _productService;

		public ProductsController(IProductsService productService)
		{
			_productService = productService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAllProducts()
		{
			var products = await _productService.GetAllProducts();
			return Ok(products);
		}

		[HttpGet("admin")]
		[Authorize(Roles = UserRoles.Admin)]
		public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAllProductsAdmin()
		{
			var products = await _productService.GetAllProductsAdmin();
			return Ok(products);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ProductResponse>> GetProductById(Guid id)
		{
			try
			{
				var product = await _productService.GetProductById(id);
				return Ok(product);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
			}
		}

		[HttpGet("category/{categoryId}")]
		public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductsByCategory(Guid categoryId)
		{
			var products = await _productService.GetProductsByCategory(categoryId);
			return Ok(products);
		}

		[HttpPost]
		[Authorize(Roles = UserRoles.Admin)]
		public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] CreateProductRequest dto)
		{
			try
			{
				var product = await _productService.CreateProduct(dto);
				return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
			}
		}

		[HttpPut("{id}")]
		[Authorize(Roles = UserRoles.Admin)]
		public async Task<ActionResult<ProductResponse>> UpdateProduct(Guid id, [FromBody] UpdateProductRequest dto)
		{
			try
			{
				var product = await _productService.UpdateProduct(id, dto);
				return Ok(product);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
			}
		}

		[HttpPatch("{id}/toggle-status")]
		[Authorize(Roles = UserRoles.Admin)]
		public async Task<IActionResult> ToggleProductStatus(Guid id)
		{
			try
			{
				await _productService.ToggleProductStatus(id);
				return NoContent();
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
			}
		}
	}
}