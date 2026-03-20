using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miniEcommerceApi.Data;
using miniEcommerceApi.DTOs.AuthDTO.Request;
using miniEcommerceApi.DTOs.CustomersDTO.Request;
using miniEcommerceApi.DTOs.CustomersDTO.Response;
using miniEcommerceApi.DTOs.Shared;
using miniEcommerceApi.Enums;
using miniEcommerceApi.Interfaces;
using miniEcommerceApi.Models;

namespace miniEcommerceApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly ITokenService _tokenService;

        public AuthService(AppDbContext context, UserManager<Users> userManager, ITokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<IEnumerable<CustomerResponse>> GetAllCustomers()
        {
            IEnumerable<Customers> Customers = await _context.Customers.Include(c => c.User).ToListAsync();
            if (!Customers.Any())
            {
                return Enumerable.Empty<CustomerResponse>();
            }
            return Customers.Select(c => new CustomerResponse
            {
                Id = c.Id,
                Name = c.FirstName,
                LastName = c.LastName,
                Cpf = c.Cpf,
                Phone = c.Phone,
                Email = c.User.Email,
                Username = c.User.UserName
            });
        }
        public async Task<CustomerResponse> GetCustomerById(Guid id)
        {
            var Customer = await _context.Customers.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
            if (Customer == null)
            {
                throw new KeyNotFoundException("Customer not found");
            }
            return new CustomerResponse
            {
                Id = Customer.Id,
                Name = Customer.FirstName,
                LastName = Customer.LastName,
                Cpf = Customer.Cpf,
                Phone = Customer.Phone,
                Email = Customer.User.Email,
                Username = Customer.User.UserName
            };
        }
        public async Task<AuthResponse> CreateCustomer(CreateCustomerRequest dto)
        {
            var existingCpf = await _context.Customers
                .FirstOrDefaultAsync(c => c.Cpf == dto.Cpf);

            if (existingCpf != null)
                throw new InvalidOperationException("CPF already in use");

            var user = new Users
            {
                Email = dto.Email,
                UserName = dto.Username,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new InvalidOperationException(result.Errors.First().Description);

            await _userManager.AddToRoleAsync(user, UserRoles.Customer);

            var Customer = new Customers
            {
                UserId = user.Id,
                FirstName = dto.Name,
                LastName = dto.LastName,
                Cpf = dto.Cpf,
                Phone = dto.Phone
            };

            try
            {
                _context.Customers.Add(Customer);
                await _context.SaveChangesAsync();
                var token = await _tokenService.GenerateToken(user, Customer.FirstName);

                return new AuthResponse
                {
                    Token = token,
                    Name = Customer.FirstName,
                    Email = user.Email
                };
            }
            catch (Exception)
            {
                await _userManager.DeleteAsync(user);
                throw new InvalidOperationException("Error creating Customer");
            }
        }
        public async Task UpdateCustomer(Guid id, UpdateCustomerRequest dto)
        {
            var Customer = await _context.Customers
        .Include(c => c.User)
        .FirstOrDefaultAsync(c => c.Id == id);

            if (Customer == null)
                throw new InvalidOperationException("Customer not found");

            if (dto.Name != null) Customer.FirstName = dto.Name;
            if (dto.LastName != null) Customer.LastName = dto.LastName;
            if (dto.Cpf != null) Customer.Cpf = dto.Cpf;
            if (dto.Phone != null) Customer.Phone = dto.Phone;

            if (dto.Email != null) Customer.User.Email = dto.Email;
            if (dto.Username != null) Customer.User.UserName = dto.Username;
            if (dto.Password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(Customer.User);
                await _userManager.ResetPasswordAsync(Customer.User, token, dto.Password);
            }

            _context.Customers.Update(Customer);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                throw new InvalidOperationException("Customer not found");

            user.IsActive = false;
            await _userManager.UpdateAsync(user);
        }
        public async Task<AuthResponse> LoginUser(LoginRequestDTO dto)
        {
            var existingUser= await _userManager.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if(existingUser == null)
            {
                throw new InvalidOperationException("Invalid credentials");
            }
            if (!existingUser.IsActive)
                throw new InvalidOperationException("User is inactive");
            var passwordValid = await _userManager.CheckPasswordAsync(existingUser, dto.Password);
            if (!passwordValid)
                throw new InvalidOperationException("Invalid credentials");

            var Customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == existingUser.Id);

            return new AuthResponse
            {
                Token = await _tokenService.GenerateToken(existingUser, existingUser.UserName),
                Name = Customer.FirstName ?? existingUser.UserName,
                Email = existingUser.Email
            };

        }
    }
}
