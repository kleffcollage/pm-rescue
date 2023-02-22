namespace PropertyMataaz.Models.InputModels
{
    public class UpdateUserModel
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public MediaModel ProfilePicture { get; set; }
        public string Bank { get; set; }
        public string AccountNumber { get; set; }
    }
}