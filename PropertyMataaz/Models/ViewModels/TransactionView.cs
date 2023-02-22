using System;

namespace PropertyMataaz.Models.ViewModels
{
    public class TransactionView
    {
        public int UserId { get; set; }
        public UserView User { get; set; }
        public int? PropertyId { get; set; }
        public int? RentReliefId { get; set; }
        public RentReliefView RentRelief { get; set; }
        public PropertyView Property { get; set; }
        public string  TransactionReference { get; set; }
        public int? PaymentLogId { get; set; }
        // public PaymentView PaymentLog { get; set; }
        public string Status { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated {get;set;}
    }
}