using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Repositories.Interfaces;

namespace PropertyMataaz.Repositories
{
    public class ReportRepository : BaseRepository<Report>, IReportRepository
    {
        public ReportRepository(PMContext context) : base(context)
        {
        }
    }
}