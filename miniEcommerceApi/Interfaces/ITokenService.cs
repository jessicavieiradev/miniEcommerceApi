using miniEcommerceApi.Models;

namespace miniEcommerceApi.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(Users user, string name);
    }
}
