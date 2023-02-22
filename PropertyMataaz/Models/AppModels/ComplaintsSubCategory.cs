using System.ComponentModel.DataAnnotations;

namespace PropertyMataaz.Models.AppModels
{
    public class ComplaintsSubCategory : BaseModel
    {
        public string Name { get; set; }
        public int ComplantsCategoryId { get; set; }
        public ComplaintsCategory ComplantsCategory { get; set; }
    }
}