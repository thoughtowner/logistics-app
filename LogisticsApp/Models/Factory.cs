using Azure;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace LogisticsApp.Models
{
    public class Factory
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string PortalUserId { get; set; }
        public PortalUser PortalUser { get; set; } = null!;

        public List<FactoryProduct> FactoryProducts { get; } = [];
    }

    public class FactoryProduct
    {
        [Key]
        public int Id { get; set; }

        public int Quantity { get; set; }
        public float Price { get; set; }

        public int FactoryId { get; set; }
        public int ProductId { get; set; }
        public Factory Factory { get; set; } = null!;
        public Product Product { get; set; } = null!;

        public List<OrderedProduct> OrderedProducts { get; } = [];
    }
}
