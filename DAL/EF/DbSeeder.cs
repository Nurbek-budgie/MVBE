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
    
    public static async Task SeedAdminUserAsync(UserManager<User> userManager)
    {
        var adminEmail = "admin@admin.com";
        var existingUser = await userManager.FindByEmailAsync(adminEmail);
        
        if (existingUser == null)
        {
            var admin = new User
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, "string");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, ERoles.Admin.ToString());
            }
        }
    }
}