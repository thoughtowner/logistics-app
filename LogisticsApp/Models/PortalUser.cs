using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;

namespace LogisticsApp.Models
{
    public class PortalUser : IdentityUser
    {
        public Factory? Factory { get; set; }
        public Shop? Shop { get; set; }
        public Truck? Truck { get; set; }
    }
}
