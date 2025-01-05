using LogisticsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticsApp.Areas.Identity.Pages.Account
{
    [Authorize]
    public class AddRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<PortalUser> _userManager;

        public AddRoleModel(RoleManager<IdentityRole> roleManager, UserManager<PortalUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [BindProperty]
        public string RoleName { get; set; }

        public List<IdentityRole> Roles { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Roles = _roleManager.Roles.ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(RoleName))
            {
                ModelState.AddModelError(string.Empty, "Пожалуйста, выберите роль.");
                return Page();
            }

            var role = await _roleManager.FindByNameAsync(RoleName);
            if (role == null)
            {
                ModelState.AddModelError(string.Empty, "Роль не существует.");
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (result.Succeeded)
            {
                return RedirectToPage("/Account/Manage/Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
