using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApp.Controllers
{
    [Authorize]
    public class LoadedProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<PortalUser> _userManager;

        public LoadedProductController(ApplicationDbContext context, UserManager<PortalUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("LoadedProducts")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var loadedProducts = await _context.LoadedProducts
                .Include(lp => lp.OrderedProduct)
                    .ThenInclude(op => op.FactoryProduct)
                        .ThenInclude(fp => fp.Product)
                .Include(lp => lp.Truck)
                .ToListAsync();

            var model = loadedProducts.Select(lp => new LoadedProductViewModel
            {
                OrderedProductId = lp.OrderedProductId,
                TruckId = lp.TruckId,
                ProductId = lp.OrderedProduct.FactoryProduct.ProductId,
                ProductName = lp.OrderedProduct.FactoryProduct.Product.Title,
                TruckBrand = lp.Truck.Brand,
                TruckModel = lp.Truck.Model,
                Quantity = lp.Quantity
            }).ToList();

            return View(model);
        }

        [Route("LoadedProducts/{orderedProductId}/{truckId}/{productId}")]
        public async Task<IActionResult> ProductDetails(int orderedProductId, int truckId, int productId)
        {
            var loadedProduct = await _context.LoadedProducts
                .Include(lp => lp.OrderedProduct)
                    .ThenInclude(op => op.FactoryProduct)
                        .ThenInclude(fp => fp.Product)
                .Include(lp => lp.Truck)
                    .ThenInclude(t => t.PortalUser)
                .FirstOrDefaultAsync(
                    lp => lp.OrderedProductId == orderedProductId &&
                    lp.TruckId == truckId &&
                    lp.OrderedProduct.FactoryProduct.ProductId == productId
                );

            if (loadedProduct == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (loadedProduct.Truck.PortalUserId != currentUser.Id && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return View(loadedProduct);
        }
    }
}
