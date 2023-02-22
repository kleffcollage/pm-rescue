using System;

namespace PropertyMataaz.Models.AppModels
{
    public class Installment : BaseModel
    {
        public double Amount { get; set; }
        public DateTime DateDue { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public int RentReliefId { get; set; }
        public RentRelief RentRelief { get; set; }
        public DateTime PaidOn { get; set; }
    }
}