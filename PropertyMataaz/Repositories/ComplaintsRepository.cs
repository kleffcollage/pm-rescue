using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Repositories
{
    public class ComplaintsRepository : BaseRepository<Complaints>, IComplaintsRepository
    {
        public ComplaintsRepository(PMContext context) : base(context)
        {
        }

        public IEnumerable<Complaints> ListComplaints()
        {
            try
            {
                var complaints = _context.Complaints.Include(c => c.Property)
                .Include(c => c.Status)
                .Include(c => c.ComplaintsSubCategory).ThenInclude(c => c.ComplantsCategory);
                return complaints;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
    }
}