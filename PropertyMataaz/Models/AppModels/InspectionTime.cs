using System;
using System.ComponentModel.DataAnnotations;

namespace PropertyMataaz.Models.AppModels
{
    public class InspectionTime : BaseModel
    {
        
        public int InspectionDateId { get; set; }
        public InspectionDate InspectionDate { get; set; }
        public DateTime AvailableTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}