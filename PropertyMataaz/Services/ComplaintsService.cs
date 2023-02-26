using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using PropertyMataaz.Controllers;
using PropertyMataaz.Models;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Services.Interfaces;
using PropertyMataaz.Utilities;
using PropertyMataaz.Utilities.Extentions;

namespace PropertyMataaz.Services
{
    public class ComplaintsService : IComplaintsService
    {
        private readonly IComplaintsRepository _complaintsRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfigurationProvider _mappingConfigurations;

        public ComplaintsService(IComplaintsRepository complaintsRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfigurationProvider mappingConfigurations)
        {
            _complaintsRepository = complaintsRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _mappingConfigurations = mappingConfigurations;
        }

        public StandardResponse<ComplaintsView> AuthorizeComplaints(int Id)
        {
            try
            {
                var complaintsToApprove = _complaintsRepository.ListComplaints().FirstOrDefault(c => c.Id == Id);
                complaintsToApprove.StatusId = (int)Statuses.APPROVED;
                complaintsToApprove = _complaintsRepository.Update(complaintsToApprove);
                return StandardResponse<ComplaintsView>.Ok(_mapper.Map<ComplaintsView>(complaintsToApprove));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<ComplaintsView>.Failed();
            }
        }

        public StandardResponse<ComplaintsView> CreateComplaints(ComplaintsModel model)
        {
            try
            {
                var UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();

                var thisComplaints = _mapper.Map<Complaints>(model);
                thisComplaints.StatusId = (int)Statuses.PENDING;
                thisComplaints.UserId = UserId;
                thisComplaints = _complaintsRepository.CreateAndReturn(thisComplaints);
                return StandardResponse<ComplaintsView>.Ok(_mapper.Map<ComplaintsView>(thisComplaints));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<ComplaintsView>.Failed();
            }
        }

        public StandardResponse<PagedCollection<ComplaintsView>> ListComplaints(PagingOptions pagingOptions, int propertyId)
        {
            try
            {
                var allComplaints = _complaintsRepository.ListComplaints().Where(c => c.PropertyId == propertyId).AsQueryable().OrderByDescending(a => a.Id).ProjectTo<ComplaintsView>(_mappingConfigurations).AsQueryable();
                var PagedResponse = allComplaints.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);
                var PagedCollection = PagedCollection<ComplaintsView>.Create(Link.ToCollection(nameof(ComplaintsController.ListComplaints)), PagedResponse.ToArray(), allComplaints.Count(), pagingOptions);
                return StandardResponse<PagedCollection<ComplaintsView>>.Ok(PagedCollection);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<ComplaintsView>>.Failed();
            }
        }

        public StandardResponse<PagedCollection<ComplaintsView>> ListAllComplaints(PagingOptions pagingOptions)
        {
            try
            {
                var allComplaints = _complaintsRepository.ListComplaints().AsQueryable().OrderByDescending(a => a.Id).ProjectTo<ComplaintsView>(_mappingConfigurations).AsQueryable();
                var PagedResponse = allComplaints.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);
                var PagedCollection = PagedCollection<ComplaintsView>.Create(Link.ToCollection(nameof(ComplaintsController.ListComplaints)), PagedResponse.ToArray(), allComplaints.Count(), pagingOptions);
                return StandardResponse<PagedCollection<ComplaintsView>>.Ok(PagedCollection);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<ComplaintsView>>.Failed();
            }
        }

        public StandardResponse<IEnumerable<ComplaintsView>> ListMyComplaints()
        {
            try
            {
                var UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
                var allComplaints = _complaintsRepository.ListComplaints().Where(c => c.UserId == UserId).AsQueryable().OrderByDescending(a => a.Id).ProjectTo<ComplaintsView>(_mappingConfigurations).AsQueryable();
                return StandardResponse<IEnumerable<ComplaintsView>>.Ok(allComplaints);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<IEnumerable<ComplaintsView>>.Failed();
            }
        }
    }
}