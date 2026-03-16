using System.ComponentModel.DataAnnotations;

namespace miniEcommerceApi.DTOs.CategoriesDTO.Request
{
    public record UpdateCategoryRequest
    (
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MinLength(3, ErrorMessage = "O nome deve ter pelo menos 3 caracteres.")]
        string Name
    );
}
