namespace LogisticsApp.Models
{
    public class LoadProductViewModel
    {
        public int OrderedProductId { get; set; }
        public string ProductName { get; set; }
        public int AvailableQuantity { get; set; }
        public int Quantity { get; set; }
    }
}
