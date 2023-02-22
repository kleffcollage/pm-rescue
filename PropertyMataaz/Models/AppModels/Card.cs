namespace PropertyMataaz.Models.AppModels
{
    public class Card : BaseModel
    {
        public string First6Digits { get; set; }
        public string Last4Digits { get; set; }
        public string Issuer { get; set; }
        public string Country { get; set; }
        public string Type { get; set; }
        public string Token { get; set; }
        public string Expiry { get; set; }
    }
}