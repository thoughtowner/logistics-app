using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

public class InitializeRoles
{
    public static async Task Initialize(IServiceProvider serviceProvider, RoleManager<IdentityRole> roleManager)
    {
        //var roleExist = await roleManager.RoleExistsAsync("User");
        //if (!roleExist)
        //{
        //    var role = new IdentityRole("User");
        //    await roleManager.CreateAsync(role);
        //}

        var roleExist = await roleManager.RoleExistsAsync("Admin");
        if (!roleExist)
        {
            var role = new IdentityRole("Admin");
            await roleManager.CreateAsync(role);
        }

        roleExist = await roleManager.RoleExistsAsync("Driver");
        if (!roleExist)
        {
            var role = new IdentityRole("Driver");
            await roleManager.CreateAsync(role);
        }

        roleExist = await roleManager.RoleExistsAsync("FactoryOwner");
        if (!roleExist)
        {
            var role = new IdentityRole("FactoryOwner");
            await roleManager.CreateAsync(role);
        }

        roleExist = await roleManager.RoleExistsAsync("ShopOwner");
        if (!roleExist)
        {
            var role = new IdentityRole("ShopOwner");
            await roleManager.CreateAsync(role);
        }
    }
}
