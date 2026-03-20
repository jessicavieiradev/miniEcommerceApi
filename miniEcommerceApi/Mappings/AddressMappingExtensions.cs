using miniEcommerceApi.DTOs.AddressDTO.Response;
using miniEcommerceApi.Models;
using System.Net;

namespace miniEcommerceApi.Mappings
{
    public static class AddressMappingExtensions
    {
        public static AddressResponse ToResponse(this Addresses address) => new AddressResponse
        {
            Id = address.Id,
            ZipCode = address.ZipCode,
            Street = address.Street,
            Number = address.Number,
            Neighborhood = address.Neighborhood,
            City = address.City,
            State = address.State
        };
    }
}
