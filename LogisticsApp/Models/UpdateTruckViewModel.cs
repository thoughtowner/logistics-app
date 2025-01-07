namespace LogisticsApp.Models
{
    public class UpdateTruckViewModel
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string StateNumber { get; set; }
        public int MaxCargoMass { get; set; }
        public int MaxCargoVolume { get; set; }
    }
}
