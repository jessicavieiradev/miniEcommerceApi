using miniEcommerceApi.DTOs.AuthDTO.Request;
using miniEcommerceApi.DTOs.CostumersDTO.Request;
using miniEcommerceApi.DTOs.CostumersDTO.Response;

namespace miniEcommerceApi.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> CreateCostumer(CreateCostumerRequest dto);
        Task UpdateCostumer(Guid id, UpdateCostumerRequest dto);
        Task DeleteUser(Guid id);
        Task<CostumerResponse> GetCostumerById(Guid id);
        Task<IEnumerable<CostumerResponse>> GetAllCostumers();
        Task<AuthResponse> LoginUser(LoginRequestDTO dto);
    }
}
