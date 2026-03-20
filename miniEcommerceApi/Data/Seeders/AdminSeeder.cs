using Microsoft.AspNetCore.Identity;
using miniEcommerceApi.Enums;
using miniEcommerceApi.Models;

namespace miniEcommerceApi.Data.Seeders
{
	public static class AdminSeeder
	{
		public static async Task SeedAsync(IServiceProvider serviceProvider, IConfiguration configuration)
		{
			var userManager = serviceProvider.GetRequiredService<UserManager<Users>>();
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

			string[] roles = { UserRoles.Admin, UserRoles.Customer };
			foreach (var role in roles)
			{
				if (!await roleManager.RoleExistsAsync(role))
					await roleManager.CreateAsync(new IdentityRole<Guid> { Name = role });
			}

			var admins = configuration.GetSection("AdminSeed").Get<List<AdminSeedConfig>>();

			if (admins == null) return;

			foreach (var adminConfig in admins)
			{
				if (await userManager.FindByEmailAsync(adminConfig.Email) != null) continue;

				var admin = new Users
				{
					Email = adminConfig.Email,
					UserName = adminConfig.UserName,
					IsActive = true,
					CreatedAt = DateTime.UtcNow
				};

				var result = await userManager.CreateAsync(admin, adminConfig.Password);
				if (result.Succeeded)
					await userManager.AddToRoleAsync(admin, UserRoles.Admin);
			}
		}

		private class AdminSeedConfig
		{
			public string Email { get; set; }
			public string UserName { get; set; }
			public string Password { get; set; }
		}
	}
}
