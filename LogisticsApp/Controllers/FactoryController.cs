using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

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
    }
}
