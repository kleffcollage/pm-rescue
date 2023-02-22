using System.Linq;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Repositories.Interfaces
{
    public interface IReportRepository
    {
         Report CreateAndReturn(Report report);
         IQueryable<Report> Query();
         Report GetById(int id);
    }
}