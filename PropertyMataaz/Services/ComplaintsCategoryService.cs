using System;
using System.Collections.Generic;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Services.Interfaces;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Services
{
    public class ComplaintsCategoryService : IComplaintsCategoryService
    {
        private readonly IComplaintsCategoryRepository _complaintsCategoryRepository;

        public ComplaintsCategoryService(IComplaintsCategoryRepository complaintsCategoryRepository)
        {
            _complaintsCategoryRepository = complaintsCategoryRepository;
        }

        public StandardResponse<ComplaintsCategory> CreateCategory(NameModel model)
        {
            try
            {
                var thisCategory = new ComplaintsCategory(){ Name = model.Name};
                var result = _complaintsCategoryRepository.CreateAndReturn(thisCategory);
                return StandardResponse<ComplaintsCategory>.Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<ComplaintsCategory>.Failed();
            }
        }

        public StandardResponse<ComplaintsSubCategory> CreateSubCategory(ComplaintsSubCategory model)
        {
            try
            {
                var result = _complaintsCategoryRepository.CreateSubCategory(model);
                return StandardResponse<ComplaintsSubCategory>.Ok(result);
                
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<ComplaintsSubCategory>.Failed();
            }

        }

        public StandardResponse<IEnumerable<ComplaintsCategory>> ListCategories()
        {
            try
            {
                var result = _complaintsCategoryRepository.ListCategories();
                return StandardResponse<IEnumerable<ComplaintsCategory>>.Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<IEnumerable<ComplaintsCategory>>.Failed();
            }
        }
    }
}