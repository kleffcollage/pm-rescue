using System.Collections;
using System.Collections.Generic;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Services.Interfaces
{
    public interface IComplaintsCategoryService
    {
         StandardResponse<ComplaintsCategory> CreateCategory(NameModel model);
         StandardResponse<ComplaintsSubCategory> CreateSubCategory(ComplaintsSubCategory model);
         StandardResponse<IEnumerable<ComplaintsCategory>> ListCategories();
    }
}