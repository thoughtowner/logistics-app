using System.ComponentModel.DataAnnotations;

namespace LogisticsApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public float Price { get; set; }
        public int Mass { get; set; }
        public int Volume { get; set; }

        public List<ShopProduct> ShopProducts { get; set; } = [];
        public List<FactoryProduct> FactoryProducts { get; set; } = [];
        public List<TruckProduct> TruckProducts { get; set; } = [];
    }
}
