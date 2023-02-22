using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Repositories
{
    public class LandSearchRepository : BaseRepository<LandSearch>, ILandSearchRepository
    {
        public LandSearchRepository(DataContext.PMContext context) : base(context)
        {
        }

        public IEnumerable<LandSearch> List()
        {
            try
            {
                var requests = _context.LandSearches.Include(l => l.Status);
                return requests;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
    }
}