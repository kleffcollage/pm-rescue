using System;

namespace PropertyMataaz.Models.ViewModels
{
    public class InspectionTimeView
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public bool Available { get; set; }
    }
}