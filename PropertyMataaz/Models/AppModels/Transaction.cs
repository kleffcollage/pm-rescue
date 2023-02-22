namespace PropertyMataaz.Models.AppModels
{
    public class Transaction : BaseModel
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int? PropertyId { get; set; }
        public int? RentReliefId { get; set; }
        public RentRelief RentRelief { get; set; }
        public Property Property { get; set; }
        public string  TransactionReference { get; set; }
        public int? PaymentLogId { get; set; }
        public PaymentLog PaymentLog { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public int? InstallmentId { get; set; }
        public Installment Installment { get; set; }
        public int? TenancyId { get; set; }
        public Tenancy Tenancy { get; set; }
    }
}