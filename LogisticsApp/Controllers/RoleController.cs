// Controllers/RoleController.cs
using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class RoleController : Controller
{
    private readonly UserManager<PortalUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;

    public RoleController(UserManager<PortalUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> AddRole()
    {
        var model = new AddRoleViewModel
        {
            Roles = await _roleManager.Roles.ToListAsync()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddRole(AddRoleViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Проверка дополнительных полей в зависимости от роли
            if (model.RoleName == "ShopOwner" && string.IsNullOrWhiteSpace(model.ShopTitle))
            {
                ModelState.AddModelError("ShopTitle", "Название магазина обязательно.");
            }
            else if (model.RoleName == "Driver" && (string.IsNullOrWhiteSpace(model.Brand) || string.IsNullOrWhiteSpace(model.Model)))
            {
                ModelState.AddModelError("Brand", "Марка и модель автомобиля обязательны для роли 'Driver'.");
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var role = await _roleManager.FindByNameAsync(model.RoleName);
                var result = await _userManager.AddToRoleAsync(user, role.Name);

                if (result.Succeeded)
                {
                    // Создаем записи для различных ролей
                    if (model.RoleName == "ShopOwner")
                    {
                        var shop = new Shop
                        {
                            Title = model.ShopTitle,
                            PortalUserId = user.Id
                        };
                        _context.Shops.Add(shop);
                    }
                    else if (model.RoleName == "Driver")
                    {
                        var truck = new Truck
                        {
                            Brand = model.Brand,
                            Model = model.Model,
                            StateNumber = model.StateNumber,
                            MaxCargoMass = model.MaxCargoMass,
                            MaxCargoVolume = model.MaxCargoVolume,
                            PortalUserId = user.Id
                        };
                        _context.Trucks.Add(truck);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Не удалось добавить роль.");
            }
        }

        model.Roles = await _roleManager.Roles.ToListAsync();
        return View(model);
    }
}
