using System.Collections.Generic;

namespace PropertyMataaz.Models.AppModels
{
    public class ComplaintsCategory : BaseModel
    {
        public string Name { get; set; }
        public IEnumerable<ComplaintsSubCategory> ComplaintsSubCategories { get; set; }
    }
}