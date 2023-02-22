using System;

namespace PropertyMataaz.Models.InputModels
{
    public class CleaningModel
    {
        public string BuildingState { get; set; }
        public int PropertyTypeId { get; set; }
        public DateTime DateNeeded { get; set; }
        public int NumberOfBathrooms { get; set; }
        public int NumberOfBedrooms { get; set; }
        public int NumberOfFloors { get; set; }
        public string BuildingType { get; set; }
        public string FileName { get; set; }
        public string FileNumber { get; set; }
        public string Location { get; set; }
    }
}