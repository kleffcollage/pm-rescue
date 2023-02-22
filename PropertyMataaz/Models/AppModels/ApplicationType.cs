namespace PropertyMataaz.Models.AppModels
{
    public class ApplicationType :BaseModel
    {
        public string Name { get; set; }

    }
        enum ApplicationTypes{ 
            BUY = 1,
            RENT,
            CLEAN,
            VERIFY,
            RELIEF

        }
}