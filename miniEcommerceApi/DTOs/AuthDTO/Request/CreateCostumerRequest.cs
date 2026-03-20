using System.ComponentModel.DataAnnotations;

namespace miniEcommerceApi.DTOs.CustomersDTO.Request
{
    public class CreateCustomerRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(50, ErrorMessage = "Username must be at most 50 characters")]
        public string Username { get; set; }

        // Customer
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(100, ErrorMessage = "First name must be at most 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(100, ErrorMessage = "Last name must be at most 100 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "SSN is required")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Cpfgit  status must be 11 characters")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [MaxLength(15, ErrorMessage = "Phone number must be at most 15 characters")]
        public string Phone { get; set; }
    }
}