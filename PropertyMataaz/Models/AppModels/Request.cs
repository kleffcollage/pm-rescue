namespace PropertyMataaz.Models.AppModels
{
    public class Request : BaseModel
    {
        public int PropertyId {get;set;}
        public Property Property { get; set; }
        public string Comment { get; set; }
        public string Budget { get; set; }
        public bool IsCleaning { get; set; }
        public bool IsFixing { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
    }
}