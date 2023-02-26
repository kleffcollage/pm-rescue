using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Repositories
{
    public class TenancyRepository : BaseRepository<Tenancy>, ITenancyRepository
    {
        public TenancyRepository(PMContext context) : base(context)
        {
        }

        public IEnumerable<Tenancy> ListTenancy()
        {
            try
            {
                return _context.Tenancies.Include(t => t.Owner).ThenInclude(t => t.PassportPhotograph).Include(t => t.Tenant).ThenInclude(t => t.PassportPhotograph).Include(t => t.Property).ThenInclude(t => t.MediaFiles).Include(t => t.Transaction).Include(t => t.Status);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        
    }
}