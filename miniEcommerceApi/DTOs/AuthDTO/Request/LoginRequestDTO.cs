using System.ComponentModel.DataAnnotations;

namespace miniEcommerceApi.DTOs.AuthDTO.Request
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Password { get; set; }
    }
}
