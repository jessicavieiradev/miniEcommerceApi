using miniEcommerceApi.DTOs.AuthDTO.Request;
using miniEcommerceApi.DTOs.CustomersDTO.Request;
using miniEcommerceApi.DTOs.CustomersDTO.Response;

namespace miniEcommerceApi.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> CreateCustomer(CreateCustomerRequest dto);
        Task UpdateCustomer(Guid id, UpdateCustomerRequest dto);
        Task DeleteUser(Guid id);
        Task<CustomerResponse> GetCustomerById(Guid id);
        Task<IEnumerable<CustomerResponse>> GetAllCustomers();
        Task<AuthResponse> LoginUser(LoginRequestDTO dto);
    }
}
