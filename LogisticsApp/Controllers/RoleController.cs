using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApp.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly UserManager<PortalUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<PortalUser> _signInManager;
        private readonly ILogger<RoleController> _logger;

        public RoleController(
            UserManager<PortalUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            SignInManager<PortalUser> signInManager,
            ILogger<RoleController> logger
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _signInManager = signInManager;
            _logger = logger;
        }

        [Route("Roles/AddRole")]
        public async Task<IActionResult> AddRole()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var availableRoles = await _roleManager.Roles
                .Where(role => !userRoles.Contains(role.Name) && role.Name != "Admin")
                .ToListAsync();

            var roleModel = new AddRoleViewModel
            {
                Roles = availableRoles
            };

            return View(roleModel);
        }

        [HttpPost]
        [Route("Roles/AddRole")]
        public async Task<IActionResult> AddRole(AddRoleViewModel roleModel)
        {
            if (string.IsNullOrEmpty(roleModel.RoleName))
            {
                ModelState.AddModelError("RoleName", "Роль должна быть выбрана.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByNameAsync(roleModel.RoleName);
            if (role == null)
            {
                ModelState.AddModelError("RoleName", "Роль не найдена.");
                return View(roleModel);
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);

                _logger.LogInformation($"User {user.UserName} successfully added to role {role.Name}");

                if (roleModel.RoleName == "ShopOwner")
                {
                    if (string.IsNullOrWhiteSpace(roleModel.ShopTitle))
                    {
                        ModelState.AddModelError("ShopTitle", "Название магазина обязательно.");
                    }
                    if (ModelState["ShopTitle"]?.Errors.Count == 0)
                    {
                        var shop = new Shop
                        {
                            Title = roleModel.ShopTitle,
                            PortalUserId = user.Id
                        };
                        _context.Shops.Add(shop);
                        await _context.SaveChangesAsync();
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
                        var factory = new Factory
                        {
                            Title = roleModel.FactoryTitle,
                            PortalUserId = user.Id
                        };
                        _context.Factories.Add(factory);
                        await _context.SaveChangesAsync();
                    }
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
                    }
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                _logger.LogError($"Error adding user {user.UserName} to role {role.Name}");
                ModelState.AddModelError("RoleName", "Не удалось добавить роль.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var availableRoles = await _roleManager.Roles
                .Where(role => !userRoles.Contains(role.Name))
                .ToListAsync();

            roleModel.Roles = availableRoles;
            return View(roleModel);
        }

        [Route("Roles/GiveUserAdminRole")]
        public async Task<IActionResult> GiveUserAdminRole()
        {
            var users = await _userManager.Users.ToListAsync();

            var nonAdminUsers = new List<SelectListItem>();

            foreach (var user in users)
            {
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                if (!isAdmin)
                {
                    nonAdminUsers.Add(new SelectListItem
                    {
                        Value = user.Id,
                        Text = user.UserName
                    });
                }
            }

            var viewModel = new GiveUserAdminRoleViewModel
            {
                Users = nonAdminUsers
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("Roles/GiveUserAdminRole")]
        public async Task<IActionResult> GiveUserAdminRole(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Пользователь не выбран.");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var isInRole = await _userManager.IsInRoleAsync(user, "Admin");

            if (!isInRole)
            {
                var result = await _userManager.AddToRoleAsync(user, "Admin");
            }

            return RedirectToAction("GiveUserAdminRole");
        }
    }
}
