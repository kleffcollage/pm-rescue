namespace PropertyMataaz.Models.AppModels
{
    public class UserEnquiry : BaseModel
    {
        public int? UserId { get; set; }
        public User User { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }
        public bool Active { get; set; }
    }
}