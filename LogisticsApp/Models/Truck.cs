using System.ComponentModel.DataAnnotations;

namespace LogisticsApp.Models
{
    public class Truck
    {
        [Key]
        public int Id { get; set; }

        public string Brand { get; set; }
        public string Model { get; set; }
        public string StateNumber { get; set; }
        public int MaxCargoMass { get; set; }
        public int MaxCargoVolume { get; set; }

        public string PortalUserId { get; set; }
        public PortalUser PortalUser { get; set; } = null!;

        public List<LoadedProduct> LoadedProducts { get; } = [];
        //public List<OrderedProduct> OrderedProducts { get; } = [];
    }

    public class LoadedProduct
    {
        [Key]
        public int Id { get; set; }

        public int Quantity { get; set; }

        public int TruckId { get; set; }
        public int OrderedProductId { get; set; }
        public Truck Truck { get; set; } = null!;
        public OrderedProduct OrderedProduct { get; set; } = null!;
    }
}
