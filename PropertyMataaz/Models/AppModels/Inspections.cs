namespace PropertyMataaz.Models.AppModels
{
    public class Inspections : BaseModel
    {
        public int InspectionDateId { get; set; }
        public int InspectionTimeId { get; set; }
        public InspectionDate InspectionDate { get; set; }
        public InspectionTime InspectionTime { get; set; }
        public int UserId { get; set; }
        public int PropertyId { get; set; }
        public User User { get; set; }
        public Property Property { get; set; }
    }
}