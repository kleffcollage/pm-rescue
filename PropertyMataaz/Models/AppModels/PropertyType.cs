namespace PropertyMataaz.Models.AppModels
{
    public class PropertyType : BaseModel
    {
        public string Name { get; set; }
    }

    enum PropertyTypes{ 
        BUNGALOW = 1,
        DUPLEX,
        FLAT,
        SEMI_DETACHED_DUPLEX,
        APARTMENT,
        TERRACE,
        OTHERS
    }
}