using System;
using System.Linq;
using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Models.SeederModels
{
    public class PropertyTitleSeeder
    {

        private readonly PMContext _context;

        public PropertyTitleSeeder(PMContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            CreateType(new PropertyTitle { Name = "C of O", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateType(new PropertyTitle { Name = "Governor's Consent", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateType(new PropertyTitle { Name = "Others", DateCreated = DateTime.Now, DateModified = DateTime.Now });
        }

        private void CreateType(PropertyTitle newType)
        {
            var existingType = _context.PropertyTitles.Where(p => p.Name == newType.Name).FirstOrDefault();
            if (existingType != null)
                return;

            _context.PropertyTitles.Add(newType);
            _context.SaveChanges();
        }
    }
}