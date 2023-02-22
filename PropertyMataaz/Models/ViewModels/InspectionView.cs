namespace PropertyMataaz.Models.ViewModels
{
    public class InspectionView
    {
        public int Id { get; set; }
        public InspectionTimeView InspectionTime { get; set; }
        public InspectionDateView InspectionDate { get; set; }
    }
}