using System.ComponentModel.DataAnnotations;

namespace miniEcommerceApi.DTOs.AddressDTO.Request
{
    public class CreateAddressRequest
    {
        [Required(ErrorMessage = "Zip code is required")]
        [StringLength(9, MinimumLength = 8, ErrorMessage = "Invalid zip code")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Number is required")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Neighborhood is required")]
        public string Neighborhood { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "State must have 2 characters")]
        public string State { get; set; }
        public CreateAddressRequest() { }

        public CreateAddressRequest(string zipCode, string street, string number, string neighborhood, string city, string state)
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
