namespace LogisticsApp.Models
{
    public class LoadedProductViewModel
    {
        public int OrderedProductId { get; set; } // Id заказа продукта
        public int TruckId { get; set; } // Id грузовика
        public int ProductId { get; set; } // Id продукта
        public string ProductName { get; set; } // Название продукта
        public string TruckBrand { get; set; } // Марка грузовика
        public string TruckModel { get; set; } // Модель грузовика
        public int Quantity { get; set; } // Количество загруженного продукта
    }
}
