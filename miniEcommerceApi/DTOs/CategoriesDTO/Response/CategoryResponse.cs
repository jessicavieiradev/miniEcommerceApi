namespace miniEcommerceApi.DTOs.CategoriesDTO.Response
{
    public record CategoryResponse
    (
        Guid Id,
        string Name,
        bool IsActive
    );
}
