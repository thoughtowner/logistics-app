using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogisticsApp.Models
{
    public class AddFactoryProductViewModel
    {
        public int FactoryId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public List<SelectListItem> Products { get; set; }
    }
}
