using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LogisticsApp.Controllers
{
    [Authorize]
    public class OrderedProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<PortalUser> _userManager;

        public OrderedProductController(ApplicationDbContext context, UserManager<PortalUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("OrderedProducts")]
        [Authorize(Roles = "Driver, Admin")]
        public async Task<IActionResult> Index()
        {
            var orderedProducts = await _context.OrderedProducts
                .Include(op => op.FactoryProduct)
                    .ThenInclude(fp => fp.Product)
                .Include(op => op.Shop)
                .ToListAsync();

            var model = orderedProducts.Select(op => new OrderedProductViewModel
            {
                Id = op.Id,
                ProductName = op.FactoryProduct.Product.Title,
                ShopName = op.Shop.Title,
                Quantity = op.Quantity,
                ProductId = op.FactoryProduct.ProductId
            }).ToList();

            return View(model);
        }

        [Route("OrderedProducts/{id}")]
        public async Task<IActionResult> ProductDetails(int id)
        {
            var orderedProduct = await _context.OrderedProducts
                .Include(op => op.FactoryProduct)
                    .ThenInclude(fp => fp.Product)
                .Include(op => op.Shop)
                .FirstOrDefaultAsync(op => op.Id == id);

            if (orderedProduct == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (orderedProduct.Shop.PortalUserId != currentUser.Id && !User.IsInRole("Driver") && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return View(orderedProduct);
        }

        [Route("OrderedProduct/{orderedProductId}/LoadProduct")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> LoadProduct(int orderedProductId)
        {
            var orderedProduct = await _context.OrderedProducts
                .Include(op => op.FactoryProduct)
                .ThenInclude(fp => fp.Product)
                .Include(op => op.Shop)
                .FirstOrDefaultAsync(op => op.Id == orderedProductId);

            if (orderedProduct == null)
            {
                return NotFound();
            }

            var model = new LoadProductViewModel
           {
                OrderedProductId = orderedProductId,
                ProductName = orderedProduct.FactoryProduct.Product.Title,
                AvailableQuantity = orderedProduct.Quantity
            };

            return View(model);
        }

        [HttpPost]
        [Route("OrderedProduct/{orderedProductId}/LoadProduct")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> LoadProduct(int orderedProductId, LoadProductViewModel model)
        {
            var orderedProduct = await _context.OrderedProducts
                .Include(op => op.FactoryProduct)
                .ThenInclude(fp => fp.Product)
                .Include(op => op.Shop)
                .FirstOrDefaultAsync(op => op.Id == orderedProductId);

            if (orderedProduct == null)
            {
                return NotFound();
            }

            model.ProductName = orderedProduct.FactoryProduct.Product.Title;
            model.AvailableQuantity = orderedProduct.Quantity;

            if (ModelState.IsValid)
            {
                orderedProduct = await _context.OrderedProducts
                    .FirstOrDefaultAsync(op => op.Id == orderedProductId);

                if (orderedProduct == null || orderedProduct.Quantity < model.Quantity)
                {
                    ModelState.AddModelError("", "Недостаточно товара для погрузки.");
                    return View(model);
                }

                var truck = await _context.Trucks
                    .FirstOrDefaultAsync(t => t.PortalUserId == User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (truck == null)
                {
                    ModelState.AddModelError("", "У вас нет связанного грузовика.");
                    return View(model);
                }

                var loadedProduct = await _context.LoadedProducts
                    .FirstOrDefaultAsync(lp => lp.TruckId == truck.Id && lp.OrderedProductId == orderedProduct.Id);

                if (loadedProduct == null)
                {
                    var newLoadedProduct = new LoadedProduct
                    {
                        TruckId = truck.Id,
                        OrderedProductId = orderedProduct.Id,
                        Quantity = model.Quantity
                    };

                    _context.LoadedProducts.Add(newLoadedProduct);
                }
                else
                {
                    loadedProduct.Quantity += model.Quantity;
                }

                orderedProduct.Quantity -= model.Quantity;

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "OrderedProducts");
            }

            return View(model);
        }
    }
}
