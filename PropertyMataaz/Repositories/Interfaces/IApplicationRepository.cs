using System.Collections.Generic;
using System.Linq;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Repositories.Interfaces
{
    public interface IApplicationRepository
    {
         Application CreateAndReturn(Application application);
         IEnumerable<Application> List();
         Application Update(Application application);
         Application Approve(Application application);
         Application Reject(Application application);
         IEnumerable<ApplicationType> ListTypes();
         Application GetPropertyApplicationsForUser(int propertyId, int userId);
         IQueryable<Application> Query();
    }
}