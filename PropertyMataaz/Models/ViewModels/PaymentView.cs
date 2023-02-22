using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Models.ViewModels
{
    public class PaymentView
    {
        public string Status { get; set; }
        public Property Property { get; set; }
        public User User { get; set; }
        public Transaction Transaction { get; set;}
        public bool IsRelief { get; set; }
    }
}