using Microsoft.AspNetCore.Identity;
using IntelligenceAgencyManagementSystem.Models;

namespace IntelligenceAgencyManagementSystem;


public class UserRoleInitializer
{
    public static async Task<Task> InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, string adminEmail, string password)
    {

        if (await roleManager.FindByNameAsync("admin") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("admin"));
        }

        if (await roleManager.FindByNameAsync("user") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("user"));
        }

        if (await userManager.FindByNameAsync(adminEmail) == null)
        {
            User admin = new User { Email = adminEmail, UserName = adminEmail };
            IdentityResult result = await userManager.CreateAsync(admin, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "admin");
            }
        }

        return new Task();
    }
}