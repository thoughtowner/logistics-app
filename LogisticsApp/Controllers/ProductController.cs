using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LogisticsApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("Products")]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();

            return View(products);
        }

        [Route("Products/{productId}")]
        public async Task<IActionResult> Details(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Route("Products/Create")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Products/Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Title = model.Title,
                    Mass = model.Mass,
                    Volume = model.Volume
                };

                _context.Add(product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [Route("Products/{id}/Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new UpdateProductViewModel
            {
                Id = product.Id,
                Title = product.Title,
                Mass = product.Mass,
                Volume = product.Volume
            };

            return View(model);
        }

        [HttpPost]
        [Route("Products/{id}/Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = await _context.Products.FindAsync(model.Id);
                if (product == null)
                {
                    return NotFound();
                }

                product.Title = model.Title;
                product.Mass = model.Mass;
                product.Volume = model.Volume;

                _context.Update(product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [Route("Products/{id}/Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .Include(p => p.ShopProducts)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [Route("Products/{id}/Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
