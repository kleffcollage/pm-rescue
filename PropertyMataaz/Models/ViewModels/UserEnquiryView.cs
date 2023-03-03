using System;
using System.Collections.Generic;

namespace PropertyMataaz.Models.ViewModels
{
    public class UserEnquiryView
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
        public bool Active { get; set; }
        public string FullName { get; set; }
        public string State { get; set; }
        public string Lga { get; set; }
        public string Area { get; set; }
        public DateTime DateCreated { get; set; }
        public List<InspectionView> Inspection { get; set; }
        public InspectionView SingleInspection { get; set; }
    }
}