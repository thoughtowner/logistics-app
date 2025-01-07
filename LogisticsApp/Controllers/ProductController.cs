using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogisticsApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product/Shop/5
        // Показываем все товары конкретного магазина по его ID
        public async Task<IActionResult> Shop(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Находим магазин по ID
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
    }
}
