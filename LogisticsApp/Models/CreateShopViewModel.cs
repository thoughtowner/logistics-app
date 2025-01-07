using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogisticsApp.Models
{
    public class CreateShopViewModel
    {
        public string Title { get; set; }
        public string PortalUserId { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }
    }
}
