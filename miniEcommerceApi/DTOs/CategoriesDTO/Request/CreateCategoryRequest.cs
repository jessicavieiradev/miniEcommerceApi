using System.ComponentModel.DataAnnotations;

namespace miniEcommerceApi.DTOs.CategoriesDTO.Request
{
    public record CreateCategoryRequest(
        [Required(ErrorMessage = "O Name é obrigatório.")]
        [MinLength(3,ErrorMessage = "Minimo de 3 caracteres.")] 
        [MaxLength(100)] 
        string Name
    );
}