using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticsApp.Areas.Identity.Pages.Account
{
    [Authorize]
    public class AddRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<PortalUser> _userManager;

        public AddRoleModel(RoleManager<IdentityRole> roleManager, UserManager<PortalUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [BindProperty]
        public string RoleName { get; set; }

        public List<IdentityRole> Roles { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Roles = _roleManager.Roles.ToList();
            ViewData["Roles"] = Roles;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromServices] ApplicationDbContext db)
        {
            if (string.IsNullOrWhiteSpace(RoleName))
            {
                ModelState.AddModelError(string.Empty, "Пожалуйста, выберите роль.");
                return Page();
            }

            var role = await _roleManager.FindByNameAsync(RoleName);
            if (role == null)
            {
                ModelState.AddModelError(string.Empty, "Роль не существует.");
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            if (await _userManager.IsInRoleAsync(user, RoleName))
            {
                ModelState.AddModelError(string.Empty, $"Пользователь уже имеет роль '{RoleName}'.");
                Roles = _roleManager.Roles.ToList();
                return Page();
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                if (RoleName == "Driver")
                {
                    var brand = Request.Form["brand"];
                    var model = Request.Form["model"];
                    var stateNumber = Request.Form["stateNumber"];
                    var maxCargoMass = int.Parse(Request.Form["maxCargoMass"]);
                    var maxCargoVolume = int.Parse(Request.Form["maxCargoVolume"]);

                    var truck = new Truck
                    {
                        Brand = brand,
                        Model = model,
                        StateNumber = stateNumber,
                        MaxCargoMass = maxCargoMass,
                        MaxCargoVolume = maxCargoVolume,
                        PortalUserId = user.Id
                    };

                    db.Trucks.Add(truck);
                }
                else if (RoleName == "ShopOwner")
                {
                    var titleShop = Request.Form["titleShop"];

                    var shop = new Shop
                    {
                        Title = titleShop,
                        PortalUserId = user.Id
                    };

                    db.Shops.Add(shop);
                }
                else if (RoleName == "FactoryOwner")
                {
                    var titleFactory = Request.Form["titleFactory"];

                    var factory = new Factory
                    {
                        Title = titleFactory,
                        PortalUserId = user.Id
                    };

                    db.Factories.Add(factory);
                }

                await db.SaveChangesAsync();

                Roles = _roleManager.Roles.ToList();

                return RedirectToPage("/Account/Manage/Index");
            }

            Roles = _roleManager.Roles.ToList();
            return Page();
        }
    }
}
