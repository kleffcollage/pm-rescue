using System;
using System.Collections.Generic;

namespace PropertyMataaz.Models.InputModels
{
    public class InspectionDateModel
    {
        public int PropertyId { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<InspectionTimeModel> Times { get; set; }
    }
}