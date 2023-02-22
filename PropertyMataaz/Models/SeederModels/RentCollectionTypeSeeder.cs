using System;
using System.Linq;
using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Models.SeederModels
{
    public class RentCollectionTypeSeeder
    {
        private readonly PMContext _context;

        public RentCollectionTypeSeeder(PMContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            CreateType(new RentCollectionType { Name = "WEEKLY", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateType(new RentCollectionType { Name = "MONTHLY", DateCreated = DateTime.Now, DateModified = DateTime.Now });
            CreateType(new RentCollectionType { Name = "YEARLY", DateCreated = DateTime.Now, DateModified = DateTime.Now });
        }

        private void CreateType(RentCollectionType newType)
        {
            var existingType = _context.RentCollectionTypes.Where(p => p.Name == newType.Name).FirstOrDefault();
            if (existingType != null)
                return;

            _context.RentCollectionTypes.Add(newType);
            _context.SaveChanges();
        }
    }
}