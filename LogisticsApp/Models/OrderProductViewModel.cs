﻿namespace LogisticsApp.Models
{
    public class OrderProductViewModel
    {
        public int ProductId { get; set; }
        public int FactoryProductId { get; set; }
        public int MaxQuantity { get; set; }
        public int Quantity { get; set; }
    }
}