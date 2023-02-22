using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Models.ViewModels
{
    public class PropertyRequestMatchView
    {
        public int Id { get; set; }
        public PropertyView Property { get; set; }
        public PropertyRequest PropertyRequest { get; set; }
        public string Status {get;set;}
    }
}