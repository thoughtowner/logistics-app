namespace LogisticsApp.Models
{
    public class LoadedProductViewModel
    {
        public int OrderedProductId { get; set; }
        public int TruckId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string TruckBrand { get; set; }
        public string TruckModel { get; set; }
        public int Quantity { get; set; }
    }
}
