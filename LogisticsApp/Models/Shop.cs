using Azure;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace LogisticsApp.Models
{
    public class Shop
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public string PortalUserId { get; set; }
        public PortalUser PortalUser { get; set; } = null!;

        public List<ShopProduct> ShopProducts { get; set; } = [];
    }

    public class ShopProduct
    {
        [Key]
        public int Id { get; set; }
        public int ShopId { get; set; }
        public int ProductId { get; set; }
        public Shop Shop { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
