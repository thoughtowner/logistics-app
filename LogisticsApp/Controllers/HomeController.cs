using LogisticsApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LogisticsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<PortalUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<PortalUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var isAuthenticated = User.Identity.IsAuthenticated;

            var roles = isAuthenticated
                ? await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User))
                : new List<string>();

            ViewData["IsAuthenticated"] = isAuthenticated;
            ViewData["Roles"] = roles;

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
