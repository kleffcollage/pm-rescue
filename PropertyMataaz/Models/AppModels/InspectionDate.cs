using System;
using System.Collections.Generic;

namespace PropertyMataaz.Models.AppModels
{
    public class InspectionDate : BaseModel
    {
        public int PropertyId { get; set; }
        public Property Property { get; set; }
        public DateTime Date { get; set;}
        public IEnumerable<InspectionTime> Times { get; set; }
        public IEnumerable<Inspections> Inspections { get; set; }
    }
}