using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Models.ViewModels
{
    public class RequestView
    {
        public int Id {get;set;}
        public Property Property { get; set; }
        public string Comment { get; set; }
        public string Budget { get; set; }
        public bool IsCleaning { get; set; }
        public bool IsFixing { get; set; }
        public string Status { get; set; }
    }
}