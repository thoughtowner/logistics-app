using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogisticsApp.Controllers
{
    [Authorize]
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("Shops")]
        [Authorize(Roles = "ShopOwner, Admin")]
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

        [Route("Shops/Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
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
        [Route("Shops/Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateShopViewModel model)
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

        [Route("Shops/{id}/Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var shop = await _context.Shops
                .Include(s => s.ShopProducts)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }

        [HttpPost]
        [Route("Shops/{id}/Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shop = await _context.Shops.FindAsync(id);
            if (shop != null)
            {
                _context.Shops.Remove(shop);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [Route("Shops/{id}/Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var shop = await _context.Shops.FindAsync(id);
            if (shop == null)
            {
                return NotFound();
            }

            var model = new UpdateShopViewModel
            {
                Id = shop.Id,
                Title = shop.Title
            };

            return View(model);
        }

        [HttpPost]
        [Route("Shops/{id}/Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateShopViewModel model)
        {
            if (ModelState.IsValid)
            {
                var shop = await _context.Shops.FindAsync(model.Id);
                if (shop == null)
                {
                    return NotFound();
                }

                shop.Title = model.Title;
                _context.Update(shop);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [Route("Shops/{id}/Products")]
        [Authorize(Roles = "ShopOwner, Admin")]
        public async Task<IActionResult> Products(int id)
        {
            var shop = await _context.Shops
                .Include(s => s.ShopProducts)
                .ThenInclude(sp => sp.Product)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }

        [Route("Shops/{shopId}/Products/{productId}")]
        [Authorize(Roles = "ShopOwner, Admin")]
        public async Task<IActionResult> ProductDetails(int shopId, int productId)
        {
            var shop = await _context.Shops
                .Include(s => s.ShopProducts)
                .ThenInclude(sp => sp.Product)
                .FirstOrDefaultAsync(s => s.Id == shopId);

            if (shop == null)
            {
                return NotFound();
            }

            var product = shop.ShopProducts
                .FirstOrDefault(sp => sp.ProductId == productId)?.Product;

            if (product == null)
            {
                return NotFound();
            }

            ViewData["ShopId"] = shopId;

            return View(product);
        }

        [Route("Shops/{shopId}/Products/AddProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(int shopId)
        {
            var shop = await _context.Shops
                .Include(s => s.ShopProducts)
                .ThenInclude(sp => sp.Product)
                .FirstOrDefaultAsync(s => s.Id == shopId);

            if (shop == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Where(p => !_context.ShopProducts.Any(sp => sp.ProductId == p.Id && sp.ShopId == shopId))
                .ToListAsync();

            var model = new AddShopProductViewModel
            {
                ShopId = shopId,
                Products = products.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Title
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [Route("Shops/{shopId}/Products/AddProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(int shopId, AddShopProductViewModel model)
        {
            if (ModelState["ShopId"]?.Errors.Count == 0 && ModelState["ProductId"]?.Errors.Count == 0 && ModelState["Quantity"]?.Errors.Count == 0)
            {
                var shop = await _context.Shops.FindAsync(model.ShopId);

                if (shop == null)
                {
                    return NotFound();
                }

                var existingProduct = await _context.ShopProducts
                    .FirstOrDefaultAsync(sp => sp.ShopId == model.ShopId && sp.ProductId == model.ProductId);

                if (existingProduct != null)
                {
                    existingProduct.Quantity += model.Quantity;
                }
                else
                {
                    var shopProduct = new ShopProduct
                    {
                        ShopId = model.ShopId,
                        ProductId = model.ProductId,
                        Quantity = model.Quantity
                    };

                    _context.ShopProducts.Add(shopProduct);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Products", new { id = model.ShopId });
            }

            var products = await _context.Products
                .Where(p => !_context.ShopProducts.Any(sp => sp.ProductId == p.Id && sp.ShopId == shopId))
                .ToListAsync();
            model.Products = products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Title
            }).ToList();

            return View(model);
        }

        [HttpGet]
        [Route("Shops/{shopId}/Products/{productId}/DeleteProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int shopId, int productId)
        {
            var shop = await _context.Shops
                .Include(s => s.ShopProducts)
                .ThenInclude(sp => sp.Product)
                .FirstOrDefaultAsync(s => s.Id == shopId);

            if (shop == null)
            {
                return NotFound();
            }

            var shopProduct = shop.ShopProducts.FirstOrDefault(sp => sp.ProductId == productId);

            if (shopProduct == null)
            {
                return NotFound();
            }

            return View(shopProduct);
        }

        [HttpPost]
        [Route("Shops/{shopId}/Products/{productId}/DeleteProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProductConfirmed(int shopId, int productId)
        {
            var shop = await _context.Shops
                .Include(s => s.ShopProducts)
                .ThenInclude(sp => sp.Product)
                .FirstOrDefaultAsync(s => s.Id == shopId);

            if (shop == null)
            {
                return NotFound();
            }

            var shopProduct = shop.ShopProducts.FirstOrDefault(sp => sp.ProductId == productId);

            if (shopProduct == null)
            {
                return NotFound();
            }

            _context.ShopProducts.Remove(shopProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction("Products", new { id = shopId });
        }

        [Route("Shops/{shopId}/Products/{productId}/UpdateProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int shopId, int productId)
        {
            var shopProduct = await _context.ShopProducts
                .FirstOrDefaultAsync(sp => sp.ShopId == shopId && sp.ProductId == productId);

            if (shopProduct == null)
            {
                return NotFound();
            }

            var model = new UpdateShopProductViewModel
            {
                ShopId = shopId,
                ProductId = productId,
                Quantity = shopProduct.Quantity
            };

            return View(model);
        }

        [HttpPost]
        [Route("Shops/{shopId}/Products/{productId}/UpdateProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(UpdateShopProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var shopProduct = await _context.ShopProducts
                    .FirstOrDefaultAsync(sp => sp.ShopId == model.ShopId && sp.ProductId == model.ProductId);

                if (shopProduct == null)
                {
                    return NotFound();
                }

                shopProduct.Quantity = model.Quantity;
                _context.ShopProducts.Update(shopProduct);
                await _context.SaveChangesAsync();

                return RedirectToAction("Products", new { id = model.ShopId });
            }

            return View(model);
        }
    }
}
