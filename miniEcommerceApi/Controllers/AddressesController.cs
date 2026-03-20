using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miniEcommerceApi.DTOs.AddressDTO.Request;
using miniEcommerceApi.DTOs.AddressDTO.Response;
using miniEcommerceApi.DTOs.Shared;
using miniEcommerceApi.Interfaces;

namespace miniEcommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressesService _addressService;

        public AddressController(IAddressesService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AddressResponse>> GetAddressById(Guid id)
        {
            try
            {
                var address = await _addressService.GetAddressById(id);
                return Ok(address);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
            }
        }

        [HttpGet("Customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<AddressResponse>>> GetAddressesByCustomerId(Guid customerId)
        {
            var addresses = await _addressService.GetAddressesByCustomerId(customerId);
            return Ok(addresses);
        }

        [HttpPost("{customerId}")]
        public async Task<ActionResult<AddressResponse>> CreateAddress(Guid customerId, [FromBody] CreateAddressRequest dto)
        {
            try
            {
                var address = await _addressService.CreateAddress(customerId, dto);
                return Ok(address);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AddressResponse>> UpdateAddress(Guid id, [FromBody] UpdateAddressRequest dto)
        {
            try
            {
                var address = await _addressService.UpdateAddress(id, dto);
                return Ok(address);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            try
            {
                await _addressService.DeleteAddress(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse(404, "Not Found", ex.Message));
            }
        }
    }
}