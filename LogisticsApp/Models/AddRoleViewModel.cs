// Models/AddRoleViewModel.cs
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LogisticsApp.Models  // Убедитесь, что пространство имен совпадает
{
    public class AddRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }

        public List<IdentityRole> Roles { get; set; }

        // Дополнительные поля для разных ролей
        public string Brand { get; set; }
        public string Model { get; set; }
        public string StateNumber { get; set; }
        public int MaxCargoMass { get; set; }
        public int MaxCargoVolume { get; set; }
        public string ShopTitle { get; set; }
        public string FactoryTitle { get; set; }
    }
}
