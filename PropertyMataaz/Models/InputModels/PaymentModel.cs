namespace PropertyMataaz.Models.InputModels
{
    public class PaymentModel
    {
        public int PropertyId { get; set; }
        public string Amount { get; set; }
        public int? RentReliefId { get; set; } = null;
        public int? InstallmentId { get; set; } = null;
    }
}