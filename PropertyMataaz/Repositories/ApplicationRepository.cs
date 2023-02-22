using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Repositories
{
    public class ApplicationRepository : BaseRepository<Application>, IApplicationRepository
    {

        public ApplicationRepository(PMContext context) : base(context)
        {
        }

        public IEnumerable<Application> List()
        {
            try
            {
                var applications = _context.Applications
                    .Include(a => a.NextOfKin)
                    .Include(a => a.ApplicationType)
                    .Include(a => a.Property)
                    .Include(a => a.Status)
                    .Include(a => a.User).ThenInclude(u => u.WorkId)
                    .Include(a => a.User).ThenInclude(u => u.PassportPhotograph);
                    
                var checker = applications.ToList();
                return checker;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public IEnumerable<ApplicationType> ListTypes()
        {
            try
            {
                var applicationTypes = _context.ApplicationTypes;
                return applicationTypes;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public Application Approve(Application application)
        {
            try
            {
                application.StatusId = (int)Statuses.APPROVED;
                Update(application);
                return application;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public Application Reject(Application application)
        {
            try
            {
                application.StatusId = (int)Statuses.REJECTED;
                Update(application);
                return application;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public Application GetPropertyApplicationsForUser(int propertyId, int userId)
        {
            try
            {
                var application = _context.Applications.Where(a => a.PropertyId == propertyId && a.UserId == userId && a.StatusId != (int)Statuses.INACTIVE).Include(a => a.Status).FirstOrDefault();
                return application;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
    }
}