using System;
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
    public class LandSearchService : ILandSearchService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILandSearchRepository _searchRepository;
        private readonly IMapper _mapper;
        private readonly IConfigurationProvider _mappingConfigurations;

        public LandSearchService(IHttpContextAccessor httpContextAccessor, IConfigurationProvider mappingConfigurations, ILandSearchRepository searchRepository, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _mappingConfigurations = mappingConfigurations;
            _searchRepository = searchRepository;
            _mapper = mapper;
        }

        public StandardResponse<LandSearchView> CreateRequest(LandSearchModel model)
        {
            try
            {
                int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
                var mappedRequest = _mapper.Map<LandSearch>(model);
                mappedRequest.UserId = UserId;
                mappedRequest.StatusId = (int)Statuses.PENDING;
                var result = _searchRepository.CreateAndReturn(mappedRequest);
                var mappedResponse = _mapper.Map<LandSearchView>(result);
                return StandardResponse<LandSearchView>.Ok(mappedResponse);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<LandSearchView>.Failed();
            }
        }

        public StandardResponse<PagedCollection<LandSearchView>> ListMyRequests(PagingOptions pagingOptions)
        {
            try
            {
                int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();

                var requests = _searchRepository.List().Where(l => l.UserId == UserId).AsQueryable().OrderByDescending(a => a.Id)
                .ProjectTo<LandSearchView>(_mappingConfigurations).AsEnumerable();

                var PagedResponse = requests.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value).ToList();

                var PagedCollection = PagedCollection<LandSearchView>.Create(Link.ToCollection(nameof(LandSearchController.ListMyLandRequests)), PagedResponse.ToArray(), requests.Count(), pagingOptions);

                return StandardResponse<PagedCollection<LandSearchView>>.Ok(PagedCollection);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<LandSearchView>>.Failed();
            }
        }

        public StandardResponse<PagedCollection<LandSearchView>> ListRequests(PagingOptions pagingOptions)
        {
            try
            {
                int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();

                var requests = _searchRepository.List().AsQueryable().OrderByDescending(a => a.Id)
                .ProjectTo<LandSearchView>(_mappingConfigurations).AsEnumerable();

                var PagedResponse = requests.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedCollection = PagedCollection<LandSearchView>.Create(Link.ToCollection(nameof(AdminController.ListAllCleanRequests)), PagedResponse.ToArray(), requests.Count(), pagingOptions);

                return StandardResponse<PagedCollection<LandSearchView>>.Ok(PagedCollection);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<LandSearchView>>.Failed();
            }
        }
    }
}