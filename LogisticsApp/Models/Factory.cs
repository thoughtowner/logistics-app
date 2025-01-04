﻿using System.ComponentModel.DataAnnotations;

namespace LogisticsApp.Models
{
    public class Factory
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public List<FactoryProduct> FactoryProducts { get; set; } = [];
    }

    public class FactoryProduct
    {
        [Key]
        public int Id { get; set; }
        public int FactoryId { get; set; }
        public int ProductId { get; set; }
        public Factory Factory { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
