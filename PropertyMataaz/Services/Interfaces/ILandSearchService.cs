using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Services.Interfaces
{
    public interface ILandSearchService
    {
         StandardResponse<LandSearchView> CreateRequest(LandSearchModel model);
         StandardResponse<PagedCollection<LandSearchView>> ListRequests(PagingOptions options);
         StandardResponse<PagedCollection<LandSearchView>> ListMyRequests(PagingOptions options);
    }
}