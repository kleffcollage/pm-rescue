using System.Collections.Generic;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Services.Interfaces
{
    public interface ITenancyService
    {
         StandardResponse<IEnumerable<TenancyView>> ListMyTenancies();
         StandardResponse<IEnumerable<TenancyView>> ListMyTenants();
        StandardResponse<PagedCollection<TenancyView>> ListAllTenancies(PagingOptions pagingOptions);
        StandardResponse<string> GetAgreement(int TenancyId);
        StandardResponse<TenancyView> ToggleRenewability(int id);
        StandardResponse<bool> UpdateTenancyAgreement(int id);
        StandardResponse<TenancyView> GetTenancy(int Id);
    }
}