using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogisticsApp.Models
{
    public class GiveUserAdminRoleViewModel
    {
        public string UserId { get; set; }
        public List<SelectListItem> Users { get; set; }
    }
}
