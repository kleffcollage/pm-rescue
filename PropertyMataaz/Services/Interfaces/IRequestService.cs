using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Services.Interfaces
{
    public interface IRequestService
    {
         StandardResponse<PagedCollection<RequestView>> ListRequests(PagingOptions pagingOptions);
         StandardResponse<PagedCollection<RequestView>> ListPendingRequests(PagingOptions pagingOptions);
        StandardResponse<PagedCollection<RequestView>> ListOngoingRequests(PagingOptions pagingOptions);
        StandardResponse<PagedCollection<RequestView>> ListResolvedRequests(PagingOptions pagingOptions);
        StandardResponse<RequestView> GetRequest(int Id);
    }
}