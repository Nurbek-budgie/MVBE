using Common.Enums;
using DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace DAL.EF;

public class DbSeeder
{
    public static async Task SeedRolesAsync(RoleManager<Role> roleManager)
    {
        foreach (var roleName in Enum.GetNames(typeof(ERoles)))
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new Role { Name = roleName });
            }
        }
    }

    public static async Task SeedAdminUserAsync(UserManager<User> userManager, string? adminEmail, string? adminPassword)
    {
        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
            return;

        if (await userManager.FindByEmailAsync(adminEmail) != null)
            return;

        var admin = new User
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, ERoles.Admin.ToString());
        }
    }
}
