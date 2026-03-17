using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miniEcommerceApi.DTOs.CategoriesDTO.Request;
using miniEcommerceApi.DTOs.CategoriesDTO.Response;
using miniEcommerceApi.DTOs.Shared;
using miniEcommerceApi.Interfaces;

namespace miniEcommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAllCategories()
        {
            var categories = await _categoriesService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponse>> GetCategoryById(Guid id)
        {
            try
            {
                var category = await _categoriesService.GetCategoryByIdAsync(id);
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponse>> CreateCategory([FromBody] CreateCategoryRequest dto)
        {
            try
            {
                var category = await _categoriesService.CreateCategoryAsync(dto);
                return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ErrorResponse(409, "Conflict", ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse(400, "Bad Request", ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryResponse>> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequest dto)
        {
            try
            {
                var category = await _categoriesService.UpdateCategoryAsync(id, dto);
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            try
            {
                await _categoriesService.DeleteCategoryAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
            }
        }
    }
}