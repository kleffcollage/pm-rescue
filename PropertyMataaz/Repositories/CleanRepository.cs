using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Repositories
{
    public class CleanRepository : BaseRepository<Cleaning>, ICleanRepository
    {
        public CleanRepository(PMContext context) : base(context)
        {
        }

        public CleaningQuote CreateCleaningQuote(CleaningQuote cleaningQuote)
        {
            try
            {
                _context.Add(cleaningQuote);
                _context.SaveChanges();
                return cleaningQuote;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public CleaningQuote GetQuoteById(int Id)
        {
            try
            {
                return _context.CleaningQuotes.FirstOrDefault(c => c.Id == Id);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public IEnumerable<Cleaning> ListCleaning()
        {
            try
            {
                var cleanings = _context.Cleanings.Include(c => c.User)
                                                  .Include(c => c.CleaningQuotes)
                                                  .Include(c => c.Status)
                                                  .Include(c => c.PropertyType);
                return cleanings;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public CleaningQuote UpdateQuote(CleaningQuote cleaningQuote)
        {
            try
            {
                _context.Update(cleaningQuote);
                return cleaningQuote;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
    }
}