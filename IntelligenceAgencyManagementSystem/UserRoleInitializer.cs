using Microsoft.AspNetCore.Identity;
using IntelligenceAgencyManagementSystem.Models;

namespace IntelligenceAgencyManagementSystem;


public class UserRoleInitializer
{
    public static async Task<Task> InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, string adminEmail, string password)
    {
        if (await roleManager.FindByNameAsync("agent") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("agent"));
        }
        
        if (await roleManager.FindByNameAsync("hr") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("hr"));
        }
        
        if (await roleManager.FindByNameAsync("commander") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("commander"));
        }
        
        if (await roleManager.FindByNameAsync("chairman") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("chairman"));
        }
        
        if (await roleManager.FindByNameAsync("admin") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("admin"));
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