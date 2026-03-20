using Microsoft.AspNetCore.Mvc;
using miniEcommerceApi.DTOs.PaymentDTO.Request;
using miniEcommerceApi.DTOs.PaymentDTO.Response;
using miniEcommerceApi.DTOs.Shared;
using miniEcommerceApi.Interfaces;

namespace miniEcommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<PaymentResponse>> GetPaymentByOrderId(Guid orderId)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByOrderId(orderId);
                return Ok(payment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<PaymentResponse>> ProcessPayment([FromBody] CreatePaymentRequest dto)
        {
            try
            {
                var payment = await _paymentService.ProcessPayment(dto);
                return Ok(payment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ErrorResponse(400, "Bad Request", ex.Message));
            }
        }
    }
}