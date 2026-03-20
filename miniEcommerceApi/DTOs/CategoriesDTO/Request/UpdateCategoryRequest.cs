using System.ComponentModel.DataAnnotations;

namespace miniEcommerceApi.DTOs.CategoriesDTO.Request
{
    public record UpdateCategoryRequest
    (
        [Required(ErrorMessage = "Name is required.")]
        [MinLength(3, ErrorMessage = "Minimum of 3 characters.")]
        string Name
    );
}
