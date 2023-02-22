namespace PropertyMataaz.Models.AppModels
{
    public class NextOfKin : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Relationship { get; set; }
        public string Address { get; set; }
    }
}