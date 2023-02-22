namespace PropertyMataaz.Models.AppModels
{
    public class LandSearch : BaseModel
    {
        
        public string FileName { get; set; }
        public string FileNumber { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
    }
}