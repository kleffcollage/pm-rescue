using System.Collections.Generic;

namespace PropertyMataaz.Models.AppModels
{
    public class PropertyRequest : BaseModel
    {
        public int PropertyTypeId { get; set; }
        public PropertyType PropertyType { get; set; }
        public string State { get; set; }
        public string LGA { get; set; }
        public int Budget { get; set; }
        public string Comment { get; set; }
        public int NumberOfBedRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public IEnumerable<PropertyRequestMatch> Matches { get; set; }
    }
}