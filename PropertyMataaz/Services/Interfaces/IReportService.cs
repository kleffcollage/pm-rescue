using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Services.Interfaces
{
    public interface IReportService
    {
         StandardResponse<ReportView> CreateReport(ReportModel model);
         StandardResponse<PagedCollection<ReportView>> GetReports(PagingOptions pagingOptions);
         StandardResponse<ReportView> GetReportById(int id);
         StandardResponse<ReportView> ContactUs(ReportModel model);
    }
}