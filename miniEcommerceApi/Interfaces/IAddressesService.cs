using miniEcommerceApi.DTOs.AddressDTO.Request;
using miniEcommerceApi.DTOs.AddressDTO.Response;

namespace miniEcommerceApi.Interfaces
{
    public interface IAddressesService
    {
        Task<AddressResponse> CreateAddress(Guid customerId, CreateAddressRequest dto);
        Task<AddressResponse> UpdateAddress(Guid id, UpdateAddressRequest dto);
        Task DeleteAddress(Guid id);
        Task<AddressResponse> GetAddressById(Guid id);
        Task<IEnumerable<AddressResponse>> GetAddressesByCustomerId(Guid customerId);
    }
}
