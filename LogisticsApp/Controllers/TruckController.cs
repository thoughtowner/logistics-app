using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace LogisticsApp.Controllers
{
    [Authorize]
    public class TruckController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<PortalUser> _userManager;

        public TruckController(ApplicationDbContext context, UserManager<PortalUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("Trucks")]
        public async Task<IActionResult> Index()
        {
            var trucks = await _context.Trucks
                .Include(t => t.PortalUser)
                .ToListAsync();

            bool isAdmin = User.IsInRole("Admin");

            var model = new TruckIndexViewModel
            {
                Trucks = trucks,
                ShowAddTruckForm = isAdmin
            };

            return View(model);
        }

        [Route("Trucks/Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var users = await _context.Users
                .Where(u => !_context.Trucks.Any(t => t.PortalUserId == u.Id))
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                })
                .ToListAsync();

            var truckModel = new CreateTruckViewModel
            {
                Users = users
            };

            return View(truckModel);
        }

        [HttpPost]
        [Route("Trucks/Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateTruckViewModel truckModel)
        {
            if (ModelState["Brand"]?.Errors.Count == 0 && ModelState["Model"]?.Errors.Count == 0 && ModelState["StateNumber"]?.Errors.Count == 0 && ModelState["MaxCargoMass"]?.Errors.Count == 0 && ModelState["MaxCargoVolume"]?.Errors.Count == 0)
            {
                var truck = new Truck
                {
                    Brand = truckModel.Brand,
                    Model = truckModel.Model,
                    StateNumber = truckModel.StateNumber,
                    MaxCargoMass = truckModel.MaxCargoMass,
                    MaxCargoVolume = truckModel.MaxCargoVolume,
                    PortalUserId = truckModel.PortalUserId
                };

                _context.Add(truck);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var usersList = await _context.Users
                .Where(u => !_context.Trucks.Any(t => t.PortalUserId == u.Id))
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                })
                .ToListAsync();

            truckModel.Users = usersList;

            return View(truckModel);
        }

        [Route("Trucks/{id}/Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var truck = await _context.Trucks
                .Include(t => t.LoadedProducts)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (truck == null)
            {
                return NotFound();
            }

            return View(truck);
        }

        [HttpPost]
        [Route("Trucks/{id}/Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var truck = await _context.Trucks.FindAsync(id);
            if (truck != null)
            {
                _context.Trucks.Remove(truck);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [Route("Trucks/{id}/Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var truck = await _context.Trucks.FindAsync(id);
            if (truck == null)
            {
                return NotFound();
            }

            var model = new UpdateTruckViewModel
            {
                Id = truck.Id,
                Brand = truck.Brand,
                Model = truck.Model,
                StateNumber = truck.StateNumber,
                MaxCargoMass = truck.MaxCargoMass,
                MaxCargoVolume = truck.MaxCargoVolume
            };

            return View(model);
        }

        [HttpPost]
        [Route("Trucks/{id}/Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateTruckViewModel truckModel)
        {
            if (ModelState.IsValid)
            {
                var truck = await _context.Trucks.FindAsync(truckModel.Id);
                if (truck == null)
                {
                    return NotFound();
                }

                truck.Brand = truckModel.Brand;
                truck.Model = truckModel.Model;
                truck.StateNumber = truckModel.StateNumber;
                truck.MaxCargoMass = truckModel.MaxCargoMass;
                truck.MaxCargoVolume = truckModel.MaxCargoVolume;
                _context.Update(truck);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(truckModel);
        }

        [Route("Trucks/{truckId}/Products")]
        public async Task<IActionResult> Products(int truckId)
        {
            var truck = await _context.Trucks
                .Include(t => t.LoadedProducts)
                    .ThenInclude(tp => tp.OrderedProduct.FactoryProduct.Product)
                .FirstOrDefaultAsync(t => t.Id == truckId);

            if (truck == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (truck.PortalUserId != currentUser?.Id && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var model = new TruckOwnerViewModel
            {
                Truck = truck,
                CurrentUserId = currentUser?.Id
            };

            return View(model);
        }

        [Route("Trucks/{truckId}/Products/{productId}")]
        public async Task<IActionResult> ProductDetails(int truckId, int productId)
        {
            var truck = await _context.Trucks
                .Include(t => t.LoadedProducts)
                .ThenInclude(tp => tp.OrderedProduct.FactoryProduct.Product)
                .FirstOrDefaultAsync(t => t.Id == truckId);

            if (truck == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (truck.PortalUserId != currentUser?.Id && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var product = truck.LoadedProducts
                .FirstOrDefault(tp => tp.OrderedProduct.FactoryProduct.ProductId == productId)?.OrderedProduct.FactoryProduct.Product;

            if (product == null)
            {
                return NotFound();
            }

            ViewData["TruckId"] = truckId;

            return View(product);
        }

        [HttpPost]
        [Route("Trucks/{truckId}/Products/DeliverAllProducts")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> DeliverProducts(int truckId)
        {
            var truck = await _context.Trucks
                .Include(t => t.LoadedProducts)
                    .ThenInclude(lp => lp.OrderedProduct)
                    .ThenInclude(op => op.FactoryProduct)
                    .ThenInclude(fp => fp.Product)
                .Include(t => t.LoadedProducts)
                    .ThenInclude(lp => lp.OrderedProduct)
                    .ThenInclude(op => op.Shop)
                .FirstOrDefaultAsync(t => t.Id == truckId);

            if (truck == null)
            {
                return NotFound();
            }

            var loadedProducts = truck.LoadedProducts.ToList();

            foreach (var loadedProduct in loadedProducts)
            {
                _context.LoadedProducts.Remove(loadedProduct);

                var orderedProduct = loadedProduct.OrderedProduct;
                var shop = orderedProduct.Shop;
                var product = orderedProduct.FactoryProduct.Product;

                var shopProduct = await _context.ShopProducts
                    .FirstOrDefaultAsync(sp => sp.ShopId == shop.Id && sp.ProductId == product.Id);

                if (shopProduct == null)
                {
                    var newShopProduct = new ShopProduct
                    {
                        ShopId = shop.Id,
                        ProductId = product.Id,
                        Quantity = loadedProduct.Quantity
                    };
                    _context.ShopProducts.Add(newShopProduct);
                }
                else
                {
                    shopProduct.Quantity += loadedProduct.Quantity;
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Products", new { truckId });
        }
    }
}
