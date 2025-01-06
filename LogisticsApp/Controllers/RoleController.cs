using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LogisticsApp.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<PortalUser> _userManager;
        private readonly ApplicationDbContext _context;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<PortalUser> userManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        // Метод для получения модели для страницы добавления роли
        [HttpGet]
        public async Task<IActionResult> AddRole()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var model = new AddRoleViewModel
            {
                Roles = roles
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleViewModel model)
        {
            // 1. Валидация полей на основе выбранной роли
            if (string.IsNullOrWhiteSpace(model.RoleName))
            {
                ModelState.AddModelError("RoleName", "Выберите роль.");
            }

            // Валидация полей для роли "Driver"
            if (model.RoleName == "Driver")
            {
                if (string.IsNullOrWhiteSpace(model.Brand))
                {
                    ModelState.AddModelError("Brand", "Марка машины обязательна для роли Driver.");
                }
                if (string.IsNullOrWhiteSpace(model.Model))
                {
                    ModelState.AddModelError("Model", "Модель машины обязательна для роли Driver.");
                }
                if (string.IsNullOrWhiteSpace(model.StateNumber))
                {
                    ModelState.AddModelError("StateNumber", "Гос. номер машины обязателен для роли Driver.");
                }
                if (model.MaxCargoMass == 0)
                {
                    ModelState.AddModelError("MaxCargoMass", "Макс. грузоподъемность обязана быть указана для роли Driver.");
                }
                if (model.MaxCargoVolume == 0)
                {
                    ModelState.AddModelError("MaxCargoVolume", "Макс. объем груза обязателен для роли Driver.");
                }
            }
            // Валидация полей для роли "ShopOwner"
            else if (model.RoleName == "ShopOwner")
            {
                if (string.IsNullOrWhiteSpace(model.ShopTitle))
                {
                    ModelState.AddModelError("ShopTitle", "Название магазина обязательно для роли ShopOwner.");
                }
            }
            // Валидация полей для роли "FactoryOwner"
            else if (model.RoleName == "FactoryOwner")
            {
                if (string.IsNullOrWhiteSpace(model.FactoryTitle))
                {
                    ModelState.AddModelError("FactoryTitle", "Название фабрики обязательно для роли FactoryOwner.");
                }
            }

            // 2. Проверка модели на ошибки
            if (!ModelState.IsValid)
            {
                // Возвращаемся на страницу с ошибками, если валидация не прошла
                model.Roles = await _roleManager.Roles.ToListAsync();
                return View(model);
            }

            // 3. Получаем пользователя, который назначает роль
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // 4. Добавляем роль пользователю
            var result = await AddRoleToUserAsync(User, model.RoleName, model);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Не удалось назначить роль.");
                model.Roles = await _roleManager.Roles.ToListAsync();
                return View(model);
            }

            // 5. Создание объектов в зависимости от роли
            if (model.RoleName == "ShopOwner")
            {
                var shop = new Shop
                {
                    Title = model.ShopTitle,  // Используем значение из формы
                    PortalUserId = user.Id
                };
                _context.Shops.Add(shop);
            }
            else if (model.RoleName == "Driver")
            {
                var truck = new Truck
                {
                    Brand = model.Brand,  // Используем значение из формы
                    Model = model.Model,
                    StateNumber = model.StateNumber,
                    MaxCargoMass = model.MaxCargoMass,
                    MaxCargoVolume = model.MaxCargoVolume,
                    PortalUserId = user.Id
                };
                _context.Trucks.Add(truck);
            }
            else if (model.RoleName == "FactoryOwner")
            {
                var factory = new Factory
                {
                    Title = model.FactoryTitle,  // Используем значение из формы
                    PortalUserId = user.Id
                };
                _context.Factories.Add(factory);
            }

            // 6. Сохраняем изменения в базе данных
            await _context.SaveChangesAsync();

            // 7. Перенаправляем на страницу управления
            return RedirectToAction("Index", "Manage");
        }

        // Метод для добавления роли пользователю
        private async Task<IdentityResult> AddRoleToUserAsync(ClaimsPrincipal userClaims, string roleName, AddRoleViewModel model)
        {
            var user = await _userManager.GetUserAsync(userClaims);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Пользователь не найден" });

            if (await _userManager.IsInRoleAsync(user, roleName))
                return IdentityResult.Failed(new IdentityError { Description = "Пользователь уже имеет эту роль" });

            // Добавляем роль пользователю
            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                // Дополнительная логика для создания объектов, связанных с ролью
                if (roleName == "Driver")
                {
                    var truck = new Truck
                    {
                        Brand = model.Brand,  // Значение из формы
                        Model = model.Model,  // Значение из формы
                        StateNumber = model.StateNumber,  // Значение из формы
                        MaxCargoMass = model.MaxCargoMass,  // Значение из формы
                        MaxCargoVolume = model.MaxCargoVolume,  // Значение из формы
                        PortalUserId = user.Id
                    };
                    _context.Trucks.Add(truck);
                }
                else if (roleName == "ShopOwner")
                {
                    var shop = new Shop
                    {
                        Title = model.ShopTitle,  // Значение из формы
                        PortalUserId = user.Id
                    };
                    _context.Shops.Add(shop);
                }
                else if (roleName == "FactoryOwner")
                {
                    var factory = new Factory
                    {
                        Title = model.FactoryTitle,  // Значение из формы
                        PortalUserId = user.Id
                    };
                    _context.Factories.Add(factory);
                }

                await _context.SaveChangesAsync();
            }

            return result;
        }
    }
}
