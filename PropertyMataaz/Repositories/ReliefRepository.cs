using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Repositories
{
    public class ReliefRepository : BaseRepository<RentRelief>, IReliefRepository
    {
        public ReliefRepository(PMContext context) : base(context)
        {
        }

        public IEnumerable<RentRelief> ListReliefs()
        {
            try
            {
                var installments = _context.RentReliefs.Include(r => r.Installments)
                                                       .ThenInclude(i => i.Status);
                return installments;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public IEnumerable<Installment> ListInstallments()
        {
            try
            {
                return _context.Installments.Include(i => i.RentRelief).Include(i => i.Status);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public Installment UpdateInstallment(Installment model)
        {
            try
            {
                _context.Update(model);
                _context.SaveChanges();
                return model;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public void CreateBulkInstallments(List<Installment> installments)
        {
            try
            {
                var allInstallments = installments.AsEnumerable();
                _context.Installments.AddRange(installments);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}