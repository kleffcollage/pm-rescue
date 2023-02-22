using System;

namespace PropertyMataaz.Models.AppModels
{
    public class PaymentLog : BaseModel
    {
        public int FlutterWavePaymentId { get; set; }
        public string TransactionReference {get; set; }
        public string FlutterWaveReference { get; set; }
        public string DeviceFingerPrint { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string ChargedAmount { get; set; }
        public string AppFee { get; set; }
        public string MerchantFee { get; set; }
        public string ProcessorResponse { get; set; }
        public string AuthModel { get; set; }
        public string IP { get; set; }
        public string Narration { get; set; }
        public string Status { get; set; }
        public string PaymentType { get; set; }
        public int AccountId { get; set; }
        public int AmountSettled { get; set;}
        public DateTime CreatedAt { get; set; }
        public int CardId { get; set; }
        public Card Card { get; set; }

    }
}