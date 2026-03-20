using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miniEcommerceApi.DTOs.AuthDTO.Request;
using miniEcommerceApi.DTOs.CustomersDTO.Request;
using miniEcommerceApi.DTOs.CustomersDTO.Response;
using miniEcommerceApi.DTOs.Shared;
using miniEcommerceApi.Enums;
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
		[Authorize(Roles = UserRoles.Admin)]
		public async Task<ActionResult<IEnumerable<CustomerResponse>>> GetAllCustomers()
		{
			var customers = await _authService.GetAllCustomers();
			if (!customers.Any())
				return NoContent();

			return Ok(customers);
		}

		[HttpGet("{id}")]
		[Authorize(Roles = UserRoles.Admin)]
		public async Task<ActionResult<CustomerResponse>> GetCustomerById(Guid id)
		{
			try
			{
				var customer = await _authService.GetCustomerById(id);
				return Ok(customer);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
			}
		}

		[HttpPost("register")]
		public async Task<ActionResult<AuthResponse>> Register([FromBody] CreateCustomerRequest dto)
		{
			try
			{
				var result = await _authService.CreateCustomer(dto);
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
		[Authorize(Roles = $"{UserRoles.Admin}, {UserRoles.Customer}")]
		public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] UpdateCustomerRequest dto)
		{
			try
			{
				await _authService.UpdateCustomer(id, dto);
				return NoContent();
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
			}
			catch (Exception ex)
			{
				return BadRequest(new ErrorResponse(400, "Bad Request", ex.Message));
			}
		}

		[HttpDelete("delete/{id}")]
		[Authorize(Roles = $"{UserRoles.Admin}, {UserRoles.Customer}")]
		public async Task<IActionResult> DeleteCustomer(Guid id)
		{
			try
			{
				await _authService.DeleteUser(id);
				return NoContent();
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
			}
			catch (Exception ex)
			{
				return BadRequest(new ErrorResponse(400, "Bad Request", ex.Message));
			}
		}
	}
}