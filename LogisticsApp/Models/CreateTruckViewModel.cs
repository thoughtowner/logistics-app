using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogisticsApp.Models
{
    public class CreateTruckViewModel
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string StateNumber { get; set; }
        public int MaxCargoMass { get; set; }
        public int MaxCargoVolume { get; set; }
        public string PortalUserId { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }
    }
}
