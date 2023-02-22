using PropertyMataaz.DataContext;

namespace PropertyMataaz.Models.SeederModels
{
    public class SeedData
    {
        private readonly PMContext _context;

        public SeedData(PMContext context)
        {
            _context = context;
        }

        public void SeedInitialData()
        {
            new PropertyTypeSeeder(_context).SeedData();
            new ApplicationTypeSeeder(_context).SeedData();
            new RentCollectionTypeSeeder(_context).SeedData();
            new StatusSeeder(_context).SeedData();
            new TenantTypeSeeder(_context).SeedData();
            new PropertyTitleSeeder(_context).SeedData();
        }
    }
}