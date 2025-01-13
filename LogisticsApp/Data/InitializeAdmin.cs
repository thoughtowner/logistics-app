using LogisticsApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticsApp.Data
{
    public static class InitializeAdmin
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<PortalUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            var adminUser = await userManager.FindByEmailAsync("admin@gmail.com");
            if (adminUser == null)
            {
                adminUser = new PortalUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com"
                };

                var result = await userManager.CreateAsync(adminUser, "admin123ADMIN!!!");
                if (result.Succeeded)
                {
                    adminUser.EmailConfirmed = true;

                    await userManager.UpdateAsync(adminUser);

                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
