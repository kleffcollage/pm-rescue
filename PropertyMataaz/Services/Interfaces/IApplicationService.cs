using System.Collections.Generic;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Services.Interfaces
{
    public interface IApplicationService
    {
        StandardResponse<ApplicationView> CreateApplication(ApplicationModel model);
        StandardResponse<PagedCollection<ApplicationView>> ListByProperty(int PropertyId, PagingOptions pagingOptions);
        StandardResponse<ApplicationView> Approve(int id);
        StandardResponse<ApplicationView> AcceptReliefApplication(int id);
        StandardResponse<ApplicationView> ReviewReliefApplication(int id);
        StandardResponse<ApplicationView> Reject(int id);
        StandardResponse<ApplicationView> GetById(int id);
        StandardResponse<IEnumerable<ApplicationType>> ListTypes();
        StandardResponse<ApplicationStatusView> GetApplication(int propertyId);
        StandardResponse<PagedCollection<Application>> ListPendingReliefApplications(PagingOptions pagingOptions);
        StandardResponse<PagedCollection<ApplicationView>> ListAcceptedReliefApplications(PagingOptions pagingOptions);
        StandardResponse<PagedCollection<ApplicationView>> ListReviewedReliefApplications(PagingOptions pagingOptions);
        StandardResponse<PagedCollection<ApplicationView>> ListApprovedReliefApplications(PagingOptions pagingOptions);
        StandardResponse<PagedCollection<Application>> ListRentApplications(PagingOptions pagingOptions);

    }
}