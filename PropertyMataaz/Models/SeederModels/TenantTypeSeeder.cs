using System;
using System.Linq;
using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Models.SeederModels
{
    public class TenantTypeSeeder
    {
        private readonly PMContext _context;

        public TenantTypeSeeder(PMContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            CreateType(new TenantType { Name = "INDIVIDUAL", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateType(new TenantType { Name = "FAMILY", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateType(new TenantType { Name = "CORPORATE", DateCreated = DateTime.Now, DateModified = DateTime.Now });
        }

        private void CreateType(TenantType newType)
        {
            var existingType = _context.TenantTypes.Where(p => p.Name == newType.Name).FirstOrDefault();
            if (existingType != null)
                return;

            _context.TenantTypes.Add(newType);
            _context.SaveChanges();
        }
    }
}