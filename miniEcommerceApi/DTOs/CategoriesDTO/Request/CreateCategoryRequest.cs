using System.ComponentModel.DataAnnotations;

namespace miniEcommerceApi.DTOs.CategoriesDTO.Request
{
    public record CreateCategoryRequest(
        [Required(ErrorMessage = "Name is required.")]
        [MinLength(3,ErrorMessage = "Minimum of 3 characters.")] 
        [MaxLength(100)] 
        string Name
    );
}