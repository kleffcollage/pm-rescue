namespace PropertyMataaz.Models.AppModels
{
    public class Complaints : BaseModel
    {
        public int ComplaintsSubCategoryId { get; set; }
        public ComplaintsSubCategory ComplaintsSubCategory { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }
}