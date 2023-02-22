using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Repositories
{
    public class ComplaintsCategoryRepository : BaseRepository<ComplaintsCategory>,  IComplaintsCategoryRepository
    {
        public ComplaintsCategoryRepository(PMContext context) : base(context)
        {
        }

        public ComplaintsSubCategory CreateSubCategory(ComplaintsSubCategory model)
        {
            try
            {
                _context.Add<ComplaintsSubCategory>(model);
                _context.SaveChanges();
                return model;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public IEnumerable<ComplaintsCategory> ListCategories()
        {
            try
            {
                var categories = _context.ComplaintsCategories.Include(c => c.ComplaintsSubCategories);
                return categories;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
    }
}