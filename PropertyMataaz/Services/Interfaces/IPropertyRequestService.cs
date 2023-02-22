using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Services.Interfaces
{
    public interface IPropertyRequestService
    {
        StandardResponse<PropertyRequestView> CreateRequest(PropertyRequestInput request);
         StandardResponse<PagedCollection<PropertyRequestView>> GetUsersRequests(PagingOptions pagingOptions);
         StandardResponse<PropertyRequestMatchView> AddMatch(int PropertyId, int RequestId);
         StandardResponse<PagedCollection<PropertyRequestView>> GetRequests(PagingOptions pagingOptions);
         StandardResponse<PropertyRequestView> GetRequest(int Id);
         StandardResponse<PropertyRequestView> RemoveMatch(int matchId);
         StandardResponse<PropertyRequestMatchView> AcceptMatch(int matchId);
         StandardResponse<PropertyRequestMatchView> RejectMatch(int matchId);
    }
}