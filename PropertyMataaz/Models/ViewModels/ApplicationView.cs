using System;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Models.ViewModels
{
    public class ApplicationView
    {
        public int Id { get; set;}
        public UserView User { get; set; }
        public NextOfKin NextOfKin { get; set; }
        public string ApplicationType { get; set; }
        public PropertyView Property {get; set; }
        public string Status { get; set; }
        public double ReliefAmount { get; set; }
        // public string RepaymentFrequency { get; set; }
        public DateTime DateCreated { get; set; }
    }
}