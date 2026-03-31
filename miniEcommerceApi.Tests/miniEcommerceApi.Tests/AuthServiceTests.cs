using Azure.Identity;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miniEcommerceApi.Data;
using miniEcommerceApi.DTOs.AuthDTO.Request;
using miniEcommerceApi.DTOs.CustomersDTO.Request;
using miniEcommerceApi.Interfaces;
using miniEcommerceApi.Models;
using miniEcommerceApi.Services;
using miniEcommerceApi.Tests.Builders;
using Moq;

namespace miniEcommerceApi.Tests
{
    public class AuthServiceTests
    {
        private AppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }
        private Mock<UserManager<Users>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<Users>>();

            return new Mock<UserManager<Users>>(
                store.Object, null!, null!, null!, null!, null!, null!, null!, null!
            );
        }
        private Mock<ITokenService> GetMockTokenService()
        {
            var mock = new Mock<ITokenService>();

            mock.Setup(t => t.GenerateToken(It.IsAny<Users>(), It.IsAny<string>()))
                .ReturnsAsync("fake-token");

            return mock;
        }

        private AuthService CreateService(AppDbContext context, Mock<UserManager<Users>>? userManager = null, Mock<ITokenService>? tokenService = null)
        {
            userManager ??= GetMockUserManager();
            tokenService ??= GetMockTokenService();
            return new AuthService(context, userManager.Object, tokenService.Object);
        }
        //Login user tests
        // - user not found, returns InvalidOperationException
        [Fact]
        public async Task LoginUser_UserNotFound_ThrowsInvalidOperationException()
        {
            //arrange
            var context = GetInMemoryContext();
            var userManager = GetMockUserManager();
            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((Users?)null);
            var service = CreateService(context, userManager);

            //act & assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.LoginUser(LoginRequestBuilder.Default()));
            exception.Message.Should().Be("Invalid credentials");
        }

        // - user is inactive, returns InvalidOperationException
        [Fact]
        public async Task LoginUser_UserInactive_ThrowsInvalidOperationException()
        {
            //arrange
            var context = GetInMemoryContext();
            var userManager = GetMockUserManager();
            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new Users { IsActive = false });
            var service = CreateService(context, userManager);
            //act & assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.LoginUser(LoginRequestBuilder.Default()));
            exception.Message.Should().Be("Invalid credentials");
        }

        // - invalid password, returns InvalidOperationException
        [Fact]
        public async Task LoginUser_InvalidPassword_ThrowsInvalidOperationException()
        {
            //arrange
            var context = GetInMemoryContext();
            var userManager = GetMockUserManager();
            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(UsersBuilder.Default());
            userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<Users>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            var service = CreateService(context, userManager);
            //act & assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.LoginUser(LoginRequestBuilder.Default()));
            exception.Message.Should().Be("Invalid credentials");
        }

        // - valid credentials, returns token, name and email
        [Fact]
        public async Task LoginUser_ValidCredentials_ReturnsAuthResponse()
        {
            var context = GetInMemoryContext();
            var userManager = GetMockUserManager();
            var user = UsersBuilder.Default();
            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<Users>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            context.Customers.Add(CustomerBuilder.Default(user.Id));
            await context.SaveChangesAsync();
            var service = CreateService(context, userManager);
            var dto = LoginRequestBuilder.Default();

            //act
            var result = await service.LoginUser(dto);

            // assert
            result.Should().NotBeNull();
            result.Token.Should().Be("fake-token");
            result.Email.Should().Be(user.Email);
        }


        //Create Customer
        // - cpf already in use, returns InvalidOperationException
        [Fact]
        public async Task CreateCustomer_CpfAlreadyInUse_ThrowsInvalidOperationException()
        {
            // arrange
            var context = GetInMemoryContext();
            var customer = CustomerBuilder.Default(Guid.NewGuid());
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var dto = new CreateCustomerRequest { Cpf = customer.Cpf };

            // act & assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.CreateCustomer(dto)
            );
            exception.Message.Should().Be("CPF already in use");
        }

        // - createasync fails, returns InvalidOperationException
        [Fact]
        public async Task CreateCustomer_CreateAsyncFails_ThrowsInvalidOperationException()
        {
            // arrange
            var context = GetInMemoryContext();
            var userManager = GetMockUserManager();
            userManager.Setup(x => x.CreateAsync(It.IsAny<Users>(), It.IsAny<string>()))
                       .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error creating user" }));

            var service = CreateService(context, userManager);
            var dto = CreateCustomerRequestBuilder.Default();

            // act & assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.CreateCustomer(dto)
            );
            exception.Message.Should().Be("Error creating user");
        }

        // - valid request, returns AuthResponse
        [Fact]
        public async Task CreateCustomer_ValidRequest_ReturnsAuthResponse()
        {
            // arrange
            var context = GetInMemoryContext();
            var userManager = GetMockUserManager();
            userManager.Setup(x => x.CreateAsync(It.IsAny<Users>(), It.IsAny<string>()))
                       .ReturnsAsync(IdentityResult.Success);
            userManager.Setup(x => x.AddToRoleAsync(It.IsAny<Users>(), It.IsAny<string>()))
                       .ReturnsAsync(IdentityResult.Success);

            var service = CreateService(context, userManager);
            var dto = CreateCustomerRequestBuilder.Default();

            // act
            var result = await service.CreateCustomer(dto);

            // assert
            result.Should().NotBeNull();
            result.Token.Should().Be("fake-token");
            result.Email.Should().Be(dto.Email);
        }


        //GetCustomerById
        // - customer not found, returns KeyNotFoundException
        [Fact]
        public async Task GetCustomerById_CustomerNotFound_ThrowsKeyNotFoundException()
        {
            // arrange
            var context = GetInMemoryContext();
            var service = CreateService(context);

            // act & assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => service.GetCustomerById(Guid.NewGuid())
            );
            exception.Message.Should().Be("Customer not found");
        }

        // - customer found, returns CustomerResponse
        [Fact]
        public async Task GetCustomerById_CustomerFound_ReturnsCustomerResponse()
        {
            // arrange
            var context = GetInMemoryContext();
            var user = UsersBuilder.Default();
            context.Users.Add(user);
            var customer = CustomerBuilder.Default(user.Id);
            customer.User = user;
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            var service = CreateService(context);

            // act
            var result = await service.GetCustomerById(customer.Id);

            // assert
            result.Should().NotBeNull();
            result.Id.Should().Be(customer.Id);
            result.Email.Should().Be(user.Email);
        }


        //DeleteUser
        // - user not found, returns InvalidOperationException
        [Fact]
        public async Task DeleteUser_UserNotFound_ThrowsInvalidOperationException()
        {
            // arrange
            var context = GetInMemoryContext();
            var userManager = GetMockUserManager();
            userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                       .ReturnsAsync((Users?)null);

            var service = CreateService(context, userManager);

            // act & assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.DeleteUser(Guid.NewGuid())
            );
            exception.Message.Should().Be("Customer not found");
        }

        // - user found, sets IsActive to false
        [Fact]
        public async Task DeleteUser_UserFound_SetsIsActiveToFalse()
        {
            // arrange
            var context = GetInMemoryContext();
            var userManager = GetMockUserManager();
            var user = UsersBuilder.Default();

            userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                       .ReturnsAsync(user);
            userManager.Setup(x => x.UpdateAsync(It.IsAny<Users>()))
                       .ReturnsAsync(IdentityResult.Success);

            var service = CreateService(context, userManager);

            // act
            await service.DeleteUser(user.Id);

            // assert
            user.IsActive.Should().BeFalse();
        }
    }
}
