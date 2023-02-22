namespace PropertyMataaz.Models.InputModels
{
    public class PropertyRequestInput
    {
        public int PropertyTypeId { get; set; }
        public string State { get; set; }
        public string LGA { get; set; }
        public int Budget { get; set; }
        public string Comment { get; set; }
        public int NumberOfBedRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
    }
}