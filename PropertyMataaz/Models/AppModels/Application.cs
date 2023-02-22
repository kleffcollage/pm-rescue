using System;

namespace PropertyMataaz.Models.AppModels
{
    public class Application : BaseModel
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int? PropertyId { get; set; }
        public Property Property { get; set; }
        public int NextOfKinId { get; set; }
        public NextOfKin NextOfKin { get; set; }
        public int ApplicationTypeId { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public double ReliefAmount { get; set; }
        public DateTime PayBackDate { get; set; }
        public string RepaymentFrequency { get; set; }
    }

    enum RepaymentFrequencies { 
        WEEKLY = 1, 
        MONTHLY = 2, 
    }
}