using System;

namespace PropertyMataaz.Models.ViewModels
{
    public class InstallmentView
    {
        public int Id {get;set;}
        public double Amount { get; set; }
        public DateTime DateDue { get; set; }
        public string Status { get; set; }
        public DateTime PaidOn { get; set; }
    }
}