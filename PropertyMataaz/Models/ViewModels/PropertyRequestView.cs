using System.Collections.Generic;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Models.ViewModels
{
    public class PropertyRequestView
    {
        public int Id { get; set; }
        public PropertyType PropertyType { get; set; }
        public string State { get; set; }
        public string LGA { get; set; }
        public int Budget { get; set; }
        public string Comment { get; set; }
        public int NumberOfBedRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public string Status { get; set; }
        public User User { get; set; }
        public IEnumerable<PropertyRequestMatchView> Matches { get; set; }
    }
}