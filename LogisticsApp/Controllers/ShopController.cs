using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogisticsApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Shop
        public async Task<IActionResult> Index()
        {
            var shops = await _context.Shops
                .Include(s => s.PortalUser)
                .ToListAsync();

            bool isAdmin = User.IsInRole("Admin");

            var model = new ShopIndexViewModel
            {
                Shops = shops,
                ShowAddShopForm = isAdmin
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateShop()
        {
            var users = await _context.Users
                .Where(u => !_context.Shops.Any(s => s.PortalUserId == u.Id))
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                })
                .ToListAsync();

            var model = new CreateShopViewModel
            {
                Users = users
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateShop(CreateShopViewModel model)
        {
            if (ModelState["Title"]?.Errors.Count == 0 && ModelState["PortalUserId"]?.Errors.Count == 0)
            {
                var shop = new Shop
                {
                    Title = model.Title,
                    PortalUserId = model.PortalUserId
                };

                _context.Add(shop);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var usersList = await _context.Users
                .Where(u => !_context.Shops.Any(s => s.PortalUserId == u.Id))
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                })
                .ToListAsync();

            model.Users = usersList;

            return View(model);
        }
    }
}
