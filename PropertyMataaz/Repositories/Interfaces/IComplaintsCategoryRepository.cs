using System.Collections.Generic;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Repositories.Interfaces
{
    public interface IComplaintsCategoryRepository
    {
         ComplaintsCategory CreateAndReturn(ComplaintsCategory model);
         ComplaintsSubCategory CreateSubCategory(ComplaintsSubCategory model);
         IEnumerable<ComplaintsCategory> ListCategories();
    }
}