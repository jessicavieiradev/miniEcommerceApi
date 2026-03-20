using Microsoft.AspNetCore.Identity;
using miniEcommerceApi.Enums;

namespace miniEcommerceApi.Data.Seeders
{
	public static class RolesSeeder
	{
		public static async Task CreateRoles(RoleManager<IdentityRole<Guid>> roleManager)
		{
			string[] roles = { UserRoles.Admin, UserRoles.Customer };

			foreach (var roleName in roles)
			{
				if (!await roleManager.RoleExistsAsync(roleName))
				{
					var result = await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));

					if (!result.Succeeded)
					{
						var erro = string.Join(", ", result.Errors.Select(e => e.Description));
						throw new Exception($"Erro ao criar role: {erro}");
					}
				}
			}
		}
	}
}
