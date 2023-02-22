using System;
using System.Collections.Generic;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Models.ViewModels
{
    public class InspectionDateView
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<InspectionTimeView> Times { get; set; }
    }
}