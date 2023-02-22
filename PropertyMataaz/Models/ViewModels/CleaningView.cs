// using System;
using System;
using System.Collections.Generic;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Models.ViewModels
{
    public class CleaningView
    {
        public int Id { get; set; }
        public string BuildingState { get; set; }
        public int PropertyTypeId { get; set; }
        public string PropertyType { get; set; }
        public DateTime DateNeeded { get; set; }
        public int NumberOfBathrooms { get; set; }
        public int NumberOfBedrooms { get; set; }
        public int NumberOfFloors { get; set; }
        public string BuildingType { get; set; }
        public string FileName { get; set; }
        public string FileNumber { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public int UserId { get; set; }
        public LeanUserView User { get; set; }
        public string Location { get; set; }
        public IEnumerable<CleaningQuoteView> CleaningQuotes { get; set; }
    }
}