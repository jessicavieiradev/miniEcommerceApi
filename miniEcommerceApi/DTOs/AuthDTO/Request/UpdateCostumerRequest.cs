using System.ComponentModel.DataAnnotations;

namespace miniEcommerceApi.DTOs.CustomersDTO.Request
{
    public class UpdateCustomerRequest
    {
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string? Email { get; set; }

        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string? Password { get; set; }

        [MaxLength(50, ErrorMessage = "Username must be at most 50 characters")]
        public string? Username { get; set; }

        // Customer
        [MaxLength(100, ErrorMessage = "First name must be at most 100 characters")]
        public string? Name { get; set; }

        [MaxLength(100, ErrorMessage = "Last name must be at most 100 characters")]
        public string? LastName { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF must be 11 characters")]
        public string? Cpf { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        [MaxLength(15, ErrorMessage = "Phone number must be at most 15 characters")]
        public string? Phone { get; set; }
    }
}