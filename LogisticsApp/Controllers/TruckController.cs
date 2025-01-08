using LogisticsApp.Data;
using LogisticsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogisticsApp.Controllers
{
    [Authorize]
    public class TruckController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TruckController(ApplicationDbContext context)
        {
            _context = context;
        }

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

        [Route("Truck/{id}/Delete")]
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
        [Route("Truck/{id}/Delete")]
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

        [Route("Truck/{id}/Update")]
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
        [Route("Truck/{id}/Update")]
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

        [Route("Truck/{id}/Products")]
        public async Task<IActionResult> Products(int id)
        {
            var truck = await _context.Trucks
                .Include(t => t.LoadedProducts)
                .ThenInclude(tp => tp.OrderedProduct.FactoryProduct.Product)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (truck == null)
            {
                return NotFound();
            }

            return View(truck);
        }

        [Route("Truck/{truckId}/Products/{productId}")]
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

            var product = truck.LoadedProducts
                .FirstOrDefault(tp => tp.OrderedProduct.FactoryProduct.ProductId == productId)?.OrderedProduct.FactoryProduct.Product;

            if (product == null)
            {
                return NotFound();
            }

            ViewData["TruckId"] = truckId;

            return View(product);
        }
    }
}
