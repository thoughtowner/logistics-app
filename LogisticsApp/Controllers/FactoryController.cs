using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace LogisticsApp.Controllers
{
    [Authorize]
    public class FactoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FactoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var factories = await _context.Factories
                .Include(f => f.PortalUser)
                .ToListAsync();

            bool isAdmin = User.IsInRole("Admin");

            var model = new FactoryIndexViewModel
            {
                Factories = factories,
                ShowAddFactoryForm = isAdmin
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var users = await _context.Users
                .Where(u => !_context.Factories.Any(f => f.PortalUserId == u.Id))
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                })
                .ToListAsync();

            var model = new CreateFactoryViewModel
            {
                Users = users
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateFactoryViewModel model)
        {
            if (ModelState["Title"]?.Errors.Count == 0 && ModelState["PortalUserId"]?.Errors.Count == 0)
            {
                var factory = new Factory
                {
                    Title = model.Title,
                    PortalUserId = model.PortalUserId
                };

                _context.Add(factory);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var usersList = await _context.Users
                .Where(u => !_context.Factories.Any(f => f.PortalUserId == u.Id))
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                })
                .ToListAsync();

            model.Users = usersList;

            return View(model);
        }

        [Route("Factory/{id}/Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var factory = await _context.Factories
                .Include(f => f.FactoryProducts)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (factory == null)
            {
                return NotFound();
            }

            return View(factory);
        }

        [HttpPost]
        [Route("Factory/{id}/Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var factory = await _context.Factories.FindAsync(id);
            if (factory != null)
            {
                _context.Factories.Remove(factory);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [Route("Factory/{id}/Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var factory = await _context.Factories.FindAsync(id);
            if (factory == null)
            {
                return NotFound();
            }

            var model = new UpdateFactoryViewModel
            {
                Id = factory.Id,
                Title = factory.Title
            };

            return View(model);
        }

        [HttpPost]
        [Route("Factory/{id}/Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateFactoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var factory = await _context.Factories.FindAsync(model.Id);
                if (factory == null)
                {
                    return NotFound();
                }

                factory.Title = model.Title;
                _context.Update(factory);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [Route("Factory/{id}/Products")]
        public async Task<IActionResult> Products(int id)
        {
            var factory = await _context.Factories
                .Include(f => f.FactoryProducts)
                .ThenInclude(fp => fp.Product)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (factory == null)
            {
                return NotFound();
            }

            return View(factory);
        }

        [Route("Factory/{factoryId}/Products/{productId}")]
        public async Task<IActionResult> ProductDetails(int factoryId, int productId)
        {
            var factory = await _context.Factories
                .Include(f => f.FactoryProducts)
                .ThenInclude(fp => fp.Product)
                .FirstOrDefaultAsync(f => f.Id == factoryId);

            if (factory == null)
            {
                return NotFound();
            }

            var product = factory.FactoryProducts
                .FirstOrDefault(fp => fp.ProductId == productId)?.Product;

            if (product == null)
            {
                return NotFound();
            }

            ViewData["FactoryId"] = factoryId;

            return View(product);
        }

        [Route("Factory/{factoryId}/Products/Add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(int factoryId)
        {
            var factory = await _context.Factories
                .Include(f => f.FactoryProducts)
                .ThenInclude(fp => fp.Product)
                .FirstOrDefaultAsync(f => f.Id == factoryId);

            if (factory == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Where(p => !_context.FactoryProducts.Any(fp => fp.ProductId == p.Id && fp.FactoryId == factoryId))
                .ToListAsync();

            var model = new AddFactoryProductViewModel
            {
                FactoryId = factoryId,
                Products = products.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Title
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [Route("Factory/{factoryId}/Products/Add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(int factoryId, AddFactoryProductViewModel model)
        {
            if (ModelState["FactoryId"]?.Errors.Count == 0 && ModelState["ProductId"]?.Errors.Count == 0 && ModelState["Quantity"]?.Errors.Count == 0)
            {
                var factory = await _context.Factories.FindAsync(model.FactoryId);

                if (factory == null)
                {
                    return NotFound();
                }

                var existingProduct = await _context.FactoryProducts
                    .FirstOrDefaultAsync(fp => fp.FactoryId == model.FactoryId && fp.ProductId == model.ProductId);

                if (existingProduct != null)
                {
                    existingProduct.Quantity += model.Quantity;
                }
                else
                {
                    var factoryProduct = new FactoryProduct
                    {
                        FactoryId = model.FactoryId,
                        ProductId = model.ProductId,
                        Quantity = model.Quantity
                    };

                    _context.FactoryProducts.Add(factoryProduct);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Products", new { id = model.FactoryId });
            }

            var products = await _context.Products
                .Where(p => !_context.FactoryProducts.Any(fp => fp.ProductId == p.Id && fp.FactoryId == factoryId))
                .ToListAsync();
            model.Products = products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Title
            }).ToList();

            return View(model);
        }

        [HttpGet]
        [Route("Factory/{factoryId}/Products/{productId}/Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int factoryId, int productId)
        {
            var factory = await _context.Factories
                .Include(f => f.FactoryProducts)
                .ThenInclude(fp => fp.Product)
                .FirstOrDefaultAsync(f => f.Id == factoryId);

            if (factory == null)
            {
                return NotFound();
            }

            var factoryProduct = factory.FactoryProducts.FirstOrDefault(fp => fp.ProductId == productId);

            if (factoryProduct == null)
            {
                return NotFound();
            }

            return View(factoryProduct);
        }

        [HttpPost]
        [Route("Factory/{factoryId}/Products/{productId}/Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProductConfirmed(int factoryId, int productId)
        {
            var factory = await _context.Factories
                .Include(f => f.FactoryProducts)
                .ThenInclude(fp => fp.Product)
                .FirstOrDefaultAsync(f => f.Id == factoryId);

            if (factory == null)
            {
                return NotFound();
            }

            var factoryProduct = factory.FactoryProducts.FirstOrDefault(fp => fp.ProductId == productId);

            if (factoryProduct == null)
            {
                return NotFound();
            }

            _context.FactoryProducts.Remove(factoryProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction("Products", new { id = factoryId });
        }

        [Route("Factory/{factoryId}/Products/{productId}/Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int factoryId, int productId)
        {
            var factoryProduct = await _context.FactoryProducts
                .FirstOrDefaultAsync(fp => fp.FactoryId == factoryId && fp.ProductId == productId);

            if (factoryProduct == null)
            {
                return NotFound();
            }

            var model = new UpdateFactoryProductViewModel
            {
                FactoryId = factoryId,
                ProductId = productId,
                Quantity = factoryProduct.Quantity
            };

            return View(model);
        }

        [HttpPost]
        [Route("Factory/{factoryId}/Products/{productId}/Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(UpdateFactoryProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var factoryProduct = await _context.FactoryProducts
                    .FirstOrDefaultAsync(fp => fp.FactoryId == model.FactoryId && fp.ProductId == model.ProductId);

                if (factoryProduct == null)
                {
                    return NotFound();
                }

                factoryProduct.Quantity = model.Quantity;
                _context.FactoryProducts.Update(factoryProduct);
                await _context.SaveChangesAsync();

                return RedirectToAction("Products", new { id = model.FactoryId });
            }

            return View(model);
        }

        [Route("Factory/{factoryId}/Products/{productId}/Order")]
        [Authorize(Roles = "ShopOwner")]
        public async Task<IActionResult> Order(int factoryId, int productId)
        {
            var factoryProduct = await _context.FactoryProducts
                .Include(fp => fp.Product)
                .FirstOrDefaultAsync(fp => fp.FactoryId == factoryId && fp.ProductId == productId);

            if (factoryProduct == null)
            {
                return NotFound();
            }

            var model = new OrderProductViewModel
            {
                ProductId = productId,
                FactoryProductId = factoryProduct.Id,
                MaxQuantity = factoryProduct.Quantity
            };

            ViewData["FactoryId"] = factoryId;

            return View(model);
        }

        [HttpPost]
        [Route("Factory/{factoryId}/Products/{productId}/Order")]
        [Authorize(Roles = "ShopOwner")]
        public async Task<IActionResult> Order(int factoryId, int productId, OrderProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var factoryProduct = await _context.FactoryProducts
                    .FirstOrDefaultAsync(fp => fp.FactoryId == factoryId && fp.ProductId == productId);

                if (factoryProduct == null || factoryProduct.Quantity < model.Quantity)
                {
                    ModelState.AddModelError("", "Недостаточно товара для заказа.");
                    return View(model);
                }

                var shop = await _context.Shops
                    .FirstOrDefaultAsync(s => s.PortalUserId == User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (shop == null)
                {
                    ModelState.AddModelError("", "У вас нет магазина, с которым можно связать заказ.");
                    return View(model);
                }

                var existingOrder = await _context.OrderedProducts
                    .FirstOrDefaultAsync(op => op.ShopId == shop.Id && op.FactoryProductId == factoryProduct.Id);

                if (existingOrder != null)
                {
                    existingOrder.Quantity += model.Quantity;
                }
                else
                {
                    var orderedProduct = new OrderedProduct
                    {
                        Quantity = model.Quantity,
                        FactoryProductId = factoryProduct.Id,
                        ShopId = shop.Id
                    };
                    _context.OrderedProducts.Add(orderedProduct);
                }

                factoryProduct.Quantity -= model.Quantity;

                await _context.SaveChangesAsync();

                return RedirectToAction("Products", new { id = factoryId });
            }

            return View(model);
        }
    }
}
