using System.ComponentModel.DataAnnotations;

namespace miniEcommerceApi.DTOs.AddressDTO.Request
{
    public class UpdateAddressRequest
    {
        [StringLength(9, MinimumLength = 8, ErrorMessage = "Invalid zip code")]
        public string? ZipCode { get; set; }
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }

        [StringLength(2, MinimumLength = 2, ErrorMessage = "State must have 2 characters")]
        public string? State { get; set; }
        public UpdateAddressRequest() { }
        public UpdateAddressRequest(string? zipCode, string? street, string? number, string? neighborhood, string? city, string? state)
        {
            ZipCode = zipCode;
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            City = city;
            State = state;
        }
    }
}
