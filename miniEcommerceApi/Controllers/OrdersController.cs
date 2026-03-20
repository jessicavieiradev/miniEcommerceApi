using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miniEcommerceApi.DTOs.OrdersDTO.Request;
using miniEcommerceApi.DTOs.OrdersDTO.Response;
using miniEcommerceApi.DTOs.Shared;
using miniEcommerceApi.Enums;
using miniEcommerceApi.Interfaces;

namespace miniEcommerceApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class OrdersController : ControllerBase
	{
		private readonly IOrderService _orderService;

		public OrdersController(IOrderService orderService)
		{
			_orderService = orderService;
		}

		[HttpGet]
		[Authorize(Roles = UserRoles.Admin)]
		public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAllOrders()
		{
			var orders = await _orderService.GetAllOrders();
			return Ok(orders);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<OrderResponse>> GetOrderById(Guid id)
		{
			try
			{
				var order = await _orderService.GetOrderById(id);
				return Ok(order);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
			}
		}

		[HttpGet("Customer/{customerId}")]
		public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByCustomerId(Guid customerId)
		{
			var orders = await _orderService.GetOrdersByCustomerId(customerId);
			return Ok(orders);
		}

		[HttpPost]
		[Authorize(Roles = UserRoles.Customer)]
		public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] CreateOrderRequest dto)
		{
			try
			{
				var order = await _orderService.CreateOrder(dto);
				return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
			}
		}

		[HttpPatch("{id}/status")]
		[Authorize(Roles = UserRoles.Admin)]
		public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusRequest dto)
		{
			try
			{
				await _orderService.UpdateOrderStatus(id, dto);
				return NoContent();
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
			}
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = $"{UserRoles.Admin}, {UserRoles.Customer}")]
		public async Task<IActionResult> DeleteOrder(Guid id)
		{
			try
			{
				await _orderService.DeleteOrder(id);
				return NoContent();
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
			}
		}
	}
}