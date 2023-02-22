using System;
using System.Linq;
using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Models.SeederModels
{
    public class StatusSeeder
    {
        private readonly PMContext _context;

        public StatusSeeder(PMContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            CreateStatus(new Status { Name = "PENDING", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateStatus(new Status { Name = "APPROVED", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateStatus(new Status { Name = "ONGOING", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateStatus(new Status { Name = "RESOLVED", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateStatus(new Status { Name = "VERIFIED", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateStatus(new Status { Name = "DRAFTED", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateStatus(new Status { Name = "ACTIVE", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateStatus(new Status { Name = "INACTIVE", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateStatus(new Status { Name = "REJECTED", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateStatus(new Status { Name = "SOLD", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateStatus(new Status { Name = "COMPLETED", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateStatus(new Status { Name = "ACCEPTED", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateStatus(new Status { Name = "REVIEWED", DateCreated = DateTime.Now, DateModified = DateTime.Now });


        }

        private void CreateStatus(Status newStatus)
        {
            var existingType = _context.Statuses.Where(p => p.Name == newStatus.Name).FirstOrDefault();
            if (existingType != null)
                return;

            _context.Statuses.Add(newStatus);
            _context.SaveChanges();
        }
    }
}