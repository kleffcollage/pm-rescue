using System;
using System.Linq;
using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Models.SeederModels
{
    public class ApplicationTypeSeeder
    {
        private readonly PMContext _context;

        public ApplicationTypeSeeder(PMContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            CreateType(new ApplicationType { Name = "BUY", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateType(new ApplicationType { Name = "RENT", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateType(new ApplicationType { Name = "CLEAN", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateType(new ApplicationType { Name = "VERIFY", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateType(new ApplicationType { Name = "RELIEF", DateCreated = DateTime.Now, DateModified = DateTime.Now });

        }

        private void CreateType(ApplicationType newType)
        {
            var existingType = _context.ApplicationTypes.Where(p => p.Name == newType.Name).FirstOrDefault();
            if (existingType != null)
                return;

            _context.ApplicationTypes.Add(newType);
            _context.SaveChanges();
        }
    }
}