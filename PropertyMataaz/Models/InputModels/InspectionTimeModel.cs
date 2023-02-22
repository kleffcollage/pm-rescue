using System;

namespace PropertyMataaz.Models.InputModels
{
    public class InspectionTimeModel
    {
        public int? id { get; set; }
        public DateTime AvailableTime { get; set; }
        public int InspectionDateId { get; set; }
    }
}