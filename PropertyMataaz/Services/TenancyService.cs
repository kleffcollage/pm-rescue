using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using PropertyMataaz.Controllers;
using PropertyMataaz.Models;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Services.Interfaces;
using PropertyMataaz.Utilities;
using PropertyMataaz.Utilities.Extentions;

namespace PropertyMataaz.Services
{
    public class TenancyService : ITenancyService
    {
        private readonly ITenancyRepository _tenancyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IConfigurationProvider _mappingConfigurations;
        private IMediaRepository _mediaRepository;
        private IMapper _mapper;

        public TenancyService(IHttpContextAccessor httpContextAccessor, ITenancyRepository tenancyRepository, IConfigurationProvider mappingConfigurations, IMediaRepository mediaRepository, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _tenancyRepository = tenancyRepository;
            _mappingConfigurations = mappingConfigurations;
            _mediaRepository = mediaRepository;
            _mapper = mapper;
        }
        public StandardResponse<PagedCollection<TenancyView>> ListAllTenancies(PagingOptions pagingOptions)
        {
            try
            {
                var loggedInUserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
                var tenancies = _tenancyRepository.ListTenancy().Where(t => t.Status.Id == (int)Statuses.ACTIVE).AsQueryable().OrderByDescending(a => a.Id).ProjectTo<TenancyView>(_mappingConfigurations).AsEnumerable();

                var PagedTenancies = tenancies.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<TenancyView>.Create(Link.ToCollection(nameof(AdminController.ListTenancies)), PagedTenancies.ToArray(), tenancies.Count(), pagingOptions);

                return StandardResponse<PagedCollection<TenancyView>>.Ok(PagedResponse);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<TenancyView>>.Failed();
            }
        }


        public StandardResponse<TenancyView> GetById(int Id)
        {
            try
            {
                var tenancies = _tenancyRepository.ListTenancy().Where(t => t.Id == Id);
                return StandardResponse<TenancyView>.Ok(_mapper.Map<TenancyView>(tenancies));

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<TenancyView>.Failed();
            }
        }

        public StandardResponse<IEnumerable<TenancyView>> ListMyTenancies()
        {
            try
            {
                var loggedInUserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
                var tenancies = _tenancyRepository.ListTenancy().Where(t => t.TenantId == loggedInUserId && t.Status.Id == (int)Statuses.ACTIVE).AsQueryable().ProjectTo<TenancyView>(_mappingConfigurations).AsEnumerable();

                return StandardResponse<IEnumerable<TenancyView>>.Ok(tenancies);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<IEnumerable<TenancyView>>.Failed();
            }
        }
        public StandardResponse<IEnumerable<TenancyView>> ListMyTenants()
        {
            try
            {
                var loggedInUserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
                var tenancies = _tenancyRepository.ListTenancy().Where(t => t.OwnerId == loggedInUserId && t.Status.Id == (int)Statuses.ACTIVE).AsQueryable().OrderByDescending(a => a.Id).ProjectTo<TenancyView>(_mappingConfigurations).AsEnumerable();

                return StandardResponse<IEnumerable<TenancyView>>.Ok(tenancies);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<IEnumerable<TenancyView>>.Failed();
            }
        }
        public StandardResponse<string> GetAgreement(int TenancyId)
        {
            var thisMedia = _mediaRepository.GetTenancyAgreement(TenancyId);
            if (thisMedia != null)
                return StandardResponse<string>.Ok(thisMedia.Url);

            return StandardResponse<string>.Error("There was an issue getting this agreement");
        }

        public StandardResponse<TenancyView> ToggleRenewability(int id)
        {
            try
            {
                var tenancy = _tenancyRepository.ListTenancy().Where(t => t.Id == id).FirstOrDefault();
                tenancy.Renewable = !tenancy.Renewable;
                _tenancyRepository.Update(tenancy);
                return StandardResponse<TenancyView>.Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<TenancyView>.Failed();
            }
        }

        public StandardResponse<bool> UpdateTenancyAgreement(int id)
        {
            try
            {
                var thisTenancy = _tenancyRepository.ListTenancy().Where(t => t.Id == id).FirstOrDefault();
                if (thisTenancy == null)
                    return StandardResponse<bool>.Error("Tenancy not found");

                thisTenancy.Agreed = true;
                _tenancyRepository.Update(thisTenancy);
                return StandardResponse<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<bool>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }
    }
}