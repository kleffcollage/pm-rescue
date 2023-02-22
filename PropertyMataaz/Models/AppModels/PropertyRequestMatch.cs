namespace PropertyMataaz.Models.AppModels
{
    public class PropertyRequestMatch : BaseModel
    {
        public int PropertyId { get; set; }
        public Property Property { get; set; }
        public int PropertyRequestId { get; set; }
        public PropertyRequest PropertyRequest { get; set; }
        public Status Status { get; set; }
        public int StatusId { get; set; }
    }
}