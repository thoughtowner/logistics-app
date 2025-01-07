using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LogisticsApp.Models
{
    public class AddRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }

        public List<IdentityRole> Roles { get; set; }

        public string Brand { get; set; }
        public string Model { get; set; }
        public string StateNumber { get; set; }
        public int MaxCargoMass { get; set; }
        public int MaxCargoVolume { get; set; }
        public string ShopTitle { get; set; }
        public string FactoryTitle { get; set; }
    }
}
