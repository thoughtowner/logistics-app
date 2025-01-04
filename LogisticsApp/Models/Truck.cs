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

        public List<TruckProduct> TruckProducts { get; } = [];
        public List<Product> Products { get; } = [];
    }

    public class TruckProduct
    {
        [Key]
        public int Id { get; set; }
        public int TruckId { get; set; }
        public int ProductId { get; set; }
        public Truck Truck { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
