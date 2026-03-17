using Microsoft.AspNetCore.Mvc;
using miniEcommerceApi.DTOs.AuthDTO.Request;
using miniEcommerceApi.DTOs.CostumersDTO.Request;
using miniEcommerceApi.DTOs.CostumersDTO.Response;
using miniEcommerceApi.DTOs.Shared;
using miniEcommerceApi.Interfaces;

namespace miniEcommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthControler : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthControler(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CostumerResponse>>> GetAllCostumers()
        {
            var costumers = await _authService.GetAllCostumers();
            if (!costumers.Any())
                return NoContent();

            return Ok(costumers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CostumerResponse>> GetCostumerById(Guid id)
        {
            var costumer = await _authService.GetCostumerById(id);
            if (costumer == null)
                return NotFound(new ErrorResponse(404, "Not Found", "Costumer not found"));

            return Ok(costumer);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] CreateCostumerRequest dto)
        {
            try
            {
                var result = await _authService.CreateCostumer(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(400, "Bad Request", ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequestDTO dto)
        {
            try
            {
                var result = await _authService.LoginUser(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(400, "Bad Request", ex.Message));
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCostumer(Guid id, [FromBody] UpdateCostumerRequest dto)
        {
            try
            {
                await _authService.UpdateCostumer(id, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(400, "Bad Request", ex.Message));
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCostumer(Guid id)
        {
            try
            {
                await _authService.DeleteUser(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(400, "Bad Request", ex.Message));
            }
        }
    }
}
