using Microsoft.AspNetCore.Identity;

namespace miniEcommerceApi.Models
{
    public class Users : IdentityUser<Guid>
    {
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Users() { }
        public Users(string email)
        {
            Id = Guid.NewGuid();
            Email = email;
            UserName = email;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }
}
