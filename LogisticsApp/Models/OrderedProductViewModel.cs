namespace LogisticsApp.Models
{
    public class OrderedProductViewModel
    {
        public int Id { get; set; } // ID заказа продукта
        public string ProductName { get; set; } // Название продукта
        public string ShopName { get; set; } // Название магазина
        public int Quantity { get; set; } // Количество
        public int ProductId { get; set; } // ID продукта
        public int ShopId { get; set; } // ID магазина
    }
}
