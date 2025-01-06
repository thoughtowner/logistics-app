using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace LogisticsApp.Models
{
    public class AddRoleViewModel
    {
        public string RoleName { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string StateNumber { get; set; }
        public int MaxCargoMass { get; set; }
        public int MaxCargoVolume { get; set; }
        public string ShopTitle { get; set; }
        public string FactoryTitle { get; set; }

        public IList<IdentityRole> Roles { get; set; }
    }
}
