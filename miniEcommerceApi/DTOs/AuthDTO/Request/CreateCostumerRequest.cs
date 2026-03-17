using System.ComponentModel.DataAnnotations;

namespace miniEcommerceApi.DTOs.CostumersDTO.Request
{
    public class CreateCostumerRequest
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [MinLength(8, ErrorMessage = "Senha deve ter no mínimo 8 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Username é obrigatório")]
        [MaxLength(50, ErrorMessage = "Username deve ter no máximo 50 caracteres")]
        public string Username { get; set; }

        // Cliente
        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Sobrenome é obrigatório")]
        [MaxLength(100, ErrorMessage = "Sobrenome deve ter no máximo 100 caracteres")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter 11 caracteres")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatório")]
        [Phone(ErrorMessage = "Telefone inválido")]
        [MaxLength(15, ErrorMessage = "Telefone deve ter no máximo 15 caracteres")]
        public string Telefone { get; set; }
    }
}
