namespace PropertyMataaz.Models.UtilityModels
{
    public class PropertyFilterOptions
    {
        public bool? Residential { get; set;} = false;
        public bool? Commercial { get; set;} = false;
        public bool? Mixed { get; set;} = false;
        public bool? Bungalow { get; set;} = false;
        public bool? Flat { get; set;} = false;
        public bool? Duplex { get; set;} = false;
        public bool? Terrace { get; set;} = false;
        public int? Bathrooms { get; set;} = 0;
        public int? Bedrooms { get; set;} = 0;
    }
}