using System;
using System.Linq;
using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Models.SeederModels
{
    public class PropertyTypeSeeder
    {
        private readonly PMContext _context;

        public PropertyTypeSeeder(PMContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            CreateType(new PropertyType{Name = "BUNGALOW",DateCreated = DateTime.Now, DateModified = DateTime.Now});
            CreateType(new PropertyType{Name = "DUPLEX",DateCreated = DateTime.Now, DateModified = DateTime.Now});
            CreateType(new PropertyType{Name = "FLAT",DateCreated = DateTime.Now, DateModified = DateTime.Now});
            CreateType(new PropertyType{Name = "SEMI-DETACHED DUPLEX",DateCreated = DateTime.Now, DateModified = DateTime.Now});
            CreateType(new PropertyType{Name = "APARTMENT",DateCreated = DateTime.Now, DateModified = DateTime.Now});
            CreateType(new PropertyType{Name = "TERRACE",DateCreated = DateTime.Now, DateModified = DateTime.Now});
            CreateType(new PropertyType{Name = "OTHERS",DateCreated = DateTime.Now, DateModified = DateTime.Now});


        }

        private void CreateType(PropertyType newType)
        {
            var existingType = _context.PropertyTypes.Where(p => p.Name == newType.Name).FirstOrDefault();
            if(existingType != null)
                return;

            _context.PropertyTypes.Add(newType);
            _context.SaveChanges();
        }
    }
}