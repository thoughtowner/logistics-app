using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LogisticsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<PortalUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<PortalUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var isAuthenticated = User.Identity.IsAuthenticated;

            ViewData["IsAuthenticated"] = isAuthenticated;

            var currentUser = await _userManager.GetUserAsync(User);

            if (User.IsInRole("FactoryOwner"))
            {
                var userFactory = await _context.Factories
                    .FirstOrDefaultAsync(f => f.PortalUserId == currentUser.Id);
                ViewData["UserFactoryId"] = userFactory?.Id;
            }

            if (User.IsInRole("ShopOwner"))
            {
                var userShop = await _context.Shops
                    .FirstOrDefaultAsync(s => s.PortalUserId == currentUser.Id);
                ViewData["UserShopId"] = userShop?.Id;
            }

            if (User.IsInRole("Driver"))
            {
                var userTruck = await _context.Trucks
                    .FirstOrDefaultAsync(t => t.PortalUserId == currentUser.Id);
                ViewData["UserTruckId"] = userTruck?.Id;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
