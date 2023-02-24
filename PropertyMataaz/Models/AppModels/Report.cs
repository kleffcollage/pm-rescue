namespace PropertyMataaz.Models.AppModels
{
    public class Report : BaseModel
    {
        public int PropertyId { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public string Description { get; set; }
        public string Email {get; set; }
        public Property Property { get; set; }
    }
}