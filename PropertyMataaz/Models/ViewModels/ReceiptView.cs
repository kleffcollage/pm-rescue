using System;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Models.ViewModels
{
    public class ReceiptView
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public PropertyView Property { get; set; }
        public PaymentLog PaymentLog { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Amount { get; set; }
        
    }
}