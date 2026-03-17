using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miniEcommerceApi.Data;
using miniEcommerceApi.DTOs.AuthDTO.Request;
using miniEcommerceApi.DTOs.CostumersDTO.Request;
using miniEcommerceApi.DTOs.CostumersDTO.Response;
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

        public async Task<IEnumerable<CostumerResponse>> GetAllCostumers()
        {
            IEnumerable<Costumers> costumers = await _context.Customers.Include(c => c.User).ToListAsync();
            if (!costumers.Any())
            {
                return Enumerable.Empty<CostumerResponse>();
            }
            return costumers.Select(c => new CostumerResponse
            {
                Id = c.Id,
                Nome = c.FirstName,
                Sobrenome = c.LastName,
                Cpf = c.Cpf,
                Telefone = c.Phone,
                Email = c.User.Email,
                Username = c.User.UserName
            });
        }
        public async Task<CostumerResponse> GetCostumerById(Guid id)
        {
            var costumer = await _context.Customers.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
            if (costumer == null)
            {
                return null;
            }
            return new CostumerResponse
            {
                Id = costumer.Id,
                Nome = costumer.FirstName,
                Sobrenome = costumer.LastName,
                Cpf = costumer.Cpf,
                Telefone = costumer.Phone,
                Email = costumer.User.Email,
                Username = costumer.User.UserName
            };
        }
        public async Task<AuthResponse> CreateCostumer(CreateCostumerRequest dto)
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

            var costumer = new Costumers
            {
                UserId = user.Id,
                FirstName = dto.Nome,
                LastName = dto.Sobrenome,
                Cpf = dto.Cpf,
                Phone = dto.Telefone
            };

            try
            {
                _context.Customers.Add(costumer);
                await _context.SaveChangesAsync();
                var token = await _tokenService.GenerateToken(user, costumer.FirstName);

                return new AuthResponse
                {
                    Token = token,
                    Nome = costumer.FirstName,
                    Email = user.Email
                };
            }
            catch (Exception)
            {
                await _userManager.DeleteAsync(user);
                throw new InvalidOperationException("Error creating costumer");
            }
        }
        public async Task UpdateCostumer(Guid id, UpdateCostumerRequest dto)
        {
            var costumer = await _context.Customers
        .Include(c => c.User)
        .FirstOrDefaultAsync(c => c.Id == id);

            if (costumer == null)
                throw new InvalidOperationException("Costumer not found");

            if (dto.Nome != null) costumer.FirstName = dto.Nome;
            if (dto.Sobrenome != null) costumer.LastName = dto.Sobrenome;
            if (dto.Cpf != null) costumer.Cpf = dto.Cpf;
            if (dto.Telefone != null) costumer.Phone = dto.Telefone;

            if (dto.Email != null) costumer.User.Email = dto.Email;
            if (dto.Username != null) costumer.User.UserName = dto.Username;
            if (dto.Password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(costumer.User);
                await _userManager.ResetPasswordAsync(costumer.User, token, dto.Password);
            }

            _context.Customers.Update(costumer);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                throw new InvalidOperationException("Costumer not found");

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

            var costumer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == existingUser.Id);

            return new AuthResponse
            {
                Token = await _tokenService.GenerateToken(existingUser, existingUser.UserName),
                Nome = costumer.FirstName ?? existingUser.UserName,
                Email = existingUser.Email
            };

        }
    }
}
