using System;
using System.Collections.Generic;

namespace PropertyMataaz.Models.AppModels
{
    public class Cleaning : BaseModel
    {
        public string BuildingState { get; set; }
        public int PropertyTypeId { get; set; }
        public PropertyType PropertyType { get; set; }
        public DateTime DateNeeded { get; set; }
        public int NumberOfBathrooms { get; set; }
        public int NumberOfBedrooms { get; set; }
        public int NumberOfFloors { get; set; }
        public string BuildingType { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int StatusId { get; set; }
        public string Location { get; set; }
        public Status Status { get; set; }
        public IEnumerable<CleaningQuote> CleaningQuotes { get; set; }
    }
}