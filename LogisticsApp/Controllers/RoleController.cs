using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApp.Controllers
{
    [Authorize]
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
            var roleModel = new AddRoleViewModel
            {
                Roles = await _roleManager.Roles.ToListAsync()
            };
            return View(roleModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleViewModel roleModel)
        {
            if (string.IsNullOrEmpty(roleModel.RoleName))
            {
                ModelState.AddModelError("RoleName", "Роль должна быть выбрана.");
            }

            if (roleModel.RoleName == "ShopOwner")
            {
                if (string.IsNullOrWhiteSpace(roleModel.ShopTitle))
                {
                    ModelState.AddModelError("ShopTitle", "Название магазина обязательно.");
                }
                if (ModelState["ShopTitle"]?.Errors.Count == 0)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var role = await _roleManager.FindByNameAsync(roleModel.RoleName);
                    var result = await _userManager.AddToRoleAsync(user, role.Name);

                    if (result.Succeeded)
                    {
                        var shop = new Shop
                        {
                            Title = roleModel.ShopTitle,
                            PortalUserId = user.Id
                        };
                        _context.Shops.Add(shop);

                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError("RoleName", "Не удалось добавить роль.");
                }
            }
            else if (roleModel.RoleName == "FactoryOwner")
            {
                if (string.IsNullOrWhiteSpace(roleModel.FactoryTitle))
                {
                    ModelState.AddModelError("FactoryTitle", "Название фабрики обязательно.");
                }
                if (ModelState["FactoryTitle"]?.Errors.Count == 0)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var role = await _roleManager.FindByNameAsync(roleModel.RoleName);
                    var result = await _userManager.AddToRoleAsync(user, role.Name);

                    if (result.Succeeded)
                    {
                        var factory = new Factory
                        {
                            Title = roleModel.FactoryTitle,
                            PortalUserId = user.Id
                        };
                        _context.Factories.Add(factory);

                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError("RoleName", "Не удалось добавить роль.");
                }
            }
            else if (roleModel.RoleName == "Admin")
            {
                var user = await _userManager.GetUserAsync(User);
                var role = await _roleManager.FindByNameAsync(roleModel.RoleName);
                var result = await _userManager.AddToRoleAsync(user, role.Name);

                if (result.Succeeded)
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("RoleName", "Не удалось добавить роль.");
            }
            else if (roleModel.RoleName == "Driver")
            {
                if (string.IsNullOrWhiteSpace(roleModel.Brand))
                {
                    ModelState.AddModelError("Brand", "Марка автомобиля обязательна для роли 'Driver'.");
                }
                if (string.IsNullOrWhiteSpace(roleModel.Model))
                {
                    ModelState.AddModelError("Model", "Модель автомобиля обязательна для роли 'Driver'.");
                }
                if (ModelState["Brand"]?.Errors.Count == 0 && ModelState["Model"]?.Errors.Count == 0)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var role = await _roleManager.FindByNameAsync(roleModel.RoleName);
                    var result = await _userManager.AddToRoleAsync(user, role.Name);

                    if (result.Succeeded)
                    {
                        var truck = new Truck
                        {
                            Brand = roleModel.Brand,
                            Model = roleModel.Model,
                            StateNumber = roleModel.StateNumber,
                            MaxCargoMass = roleModel.MaxCargoMass,
                            MaxCargoVolume = roleModel.MaxCargoVolume,
                            PortalUserId = user.Id
                        };
                        _context.Trucks.Add(truck);

                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError("RoleName", "Не удалось добавить роль.");
                }
            }

            roleModel.Roles = await _roleManager.Roles.ToListAsync();
            return View(roleModel);
        }
    }
}
