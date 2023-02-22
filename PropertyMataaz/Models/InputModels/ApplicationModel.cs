using System;

namespace PropertyMataaz.Models.InputModels
{
    public class ApplicationModel
    {
        public Register Register { get; set; }
        public NextOfKinModel NextOfKin { get; set; }
        public int ApplicationTypeId { get; set; }
        public int PropertyId { get; set; }
        public double ReliefAmount { get; set; }
        public DateTime PayBackDate { get; set; }
        public string RepaymentFrequency { get; set; }
    }
}