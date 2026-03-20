using System.ComponentModel.DataAnnotations;

namespace miniEcommerceApi.DTOs.CustomersDTO.Request
{
    public class UpdateCustomerRequest
    {
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string? Email { get; set; }

        [MinLength(8, ErrorMessage = "Senha deve ter no mínimo 8 caracteres")]
        public string? Password { get; set; }

        [MaxLength(50, ErrorMessage = "Username deve ter no máximo 50 caracteres")]
        public string? Username { get; set; }

        // Cliente
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string? Nome { get; set; }

        [MaxLength(100, ErrorMessage = "Sobrenome deve ter no máximo 100 caracteres")]
        public string? Sobrenome { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter 11 caracteres")]
        public string? Cpf { get; set; }

        [Phone(ErrorMessage = "Telefone inválido")]
        [MaxLength(15, ErrorMessage = "Telefone deve ter no máximo 15 caracteres")]
        public string? Telefone { get; set; }
    }
}
