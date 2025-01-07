using System.ComponentModel.DataAnnotations;

namespace LogisticsApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public int Mass { get; set; }
        public int Volume { get; set; }

        public List<ShopProduct> ShopProducts { get; } = [];

        public List<FactoryProduct> FactoryProducts { get; } = [];
    }
}
