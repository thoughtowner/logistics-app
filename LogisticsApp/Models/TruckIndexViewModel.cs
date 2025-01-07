namespace LogisticsApp.Models
{
    public class TruckIndexViewModel
    {
        public IEnumerable<Truck> Trucks { get; set; }
        public bool ShowAddTruckForm { get; set; }
    }
}
