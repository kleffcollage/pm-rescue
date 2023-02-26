using System.Collections.Generic;
using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Services.Interfaces
{
    public interface IComplaintsService
    {
        StandardResponse<ComplaintsView> CreateComplaints(ComplaintsModel model);
        StandardResponse<IEnumerable<ComplaintsView>> ListMyComplaints();
        StandardResponse<ComplaintsView> AuthorizeComplaints(int Id);
        StandardResponse<PagedCollection<ComplaintsView>> ListComplaints(PagingOptions pagingOptions, int propertyId);
        StandardResponse<PagedCollection<ComplaintsView>> ListAllComplaints(PagingOptions pagingOptions);
    }
}