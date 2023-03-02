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
    public class PropertyRequestService : IPropertyRequestService
    {
        private readonly IMapper _mapper;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public IConfigurationProvider _mappingConfigurations;
        public IPropertyRequestRepository _propertyRequestRepository;
        public PropertyRequestService(IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfigurationProvider mappingConfigurations, IPropertyRequestRepository propertyRequestRepository)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _mappingConfigurations = mappingConfigurations;
            _propertyRequestRepository = propertyRequestRepository;
        }

        public StandardResponse<PropertyRequestView> CreateRequest(PropertyRequestInput request)
        {
            try
            {
                int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
                var mappedRequest = _mapper.Map<PropertyRequest>(request);
                mappedRequest.UserId = UserId;
                mappedRequest.StatusId = (int)Statuses.PENDING;

                var result = _propertyRequestRepository.CreateAndReturn(mappedRequest);

                return StandardResponse<PropertyRequestView>.Ok(_mapper.Map<PropertyRequestView>(result));

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PropertyRequestView>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }


        public StandardResponse<PagedCollection<PropertyRequestView>> GetUsersRequests(PagingOptions pagingOptions)
        {
            try
            {
                int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
                var requests = _propertyRequestRepository.GetRequests().AsQueryable().OrderByDescending(a => a.Id)
                                    .ProjectTo<PropertyRequestView>(_mappingConfigurations)
                                    .AsEnumerable().Where(p => p.User.Id == UserId);

                var PagedRequests = requests.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<PropertyRequestView>.Create(Link.ToCollection(nameof(PropertyRequestController.ListUsersRequests)), PagedRequests.ToArray(), requests.Count(), pagingOptions);

                return StandardResponse<PagedCollection<PropertyRequestView>>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL).AddData(PagedResponse);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<PropertyRequestView>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PagedCollection<PropertyRequestView>> GetUsersRequestsAdmin(PagingOptions pagingOptions, int UserId)
        {
            try
            {
                var requests = _propertyRequestRepository.GetRequests().AsQueryable().OrderByDescending(a => a.Id)
                                    .ProjectTo<PropertyRequestView>(_mappingConfigurations)
                                    .AsEnumerable().Where(p => p.User.Id == UserId);

                var PagedRequests = requests.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<PropertyRequestView>.Create(Link.ToCollection(nameof(PropertyRequestController.ListUsersRequests)), PagedRequests.ToArray(), requests.Count(), pagingOptions);

                return StandardResponse<PagedCollection<PropertyRequestView>>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL).AddData(PagedResponse);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<PropertyRequestView>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PropertyRequestView> RemoveMatch(int matchId)
        {
            try{
                var result = _propertyRequestRepository.RemoveRequestMatch(matchId);
                if(!result)
                    return StandardResponse<PropertyRequestView>.Error(StandardResponseMessages.ERROR_OCCURRED);
                
                return StandardResponse<PropertyRequestView>.Ok();

            }
            catch(Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PropertyRequestView>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }

        }

        public StandardResponse<PagedCollection<PropertyRequestView>> GetRequests(PagingOptions pagingOptions)
        {
            try
            {
                var requests = _propertyRequestRepository.GetRequests().AsQueryable().OrderByDescending(a => a.Id)
                                    .ProjectTo<PropertyRequestView>(_mappingConfigurations)
                                    .AsEnumerable();

                var PagedRequests = requests.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<PropertyRequestView>.Create(Link.ToCollection(nameof(PropertyRequestController.ListUsersRequests)), PagedRequests.ToArray(), requests.Count(), pagingOptions);

                return StandardResponse<PagedCollection<PropertyRequestView>>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL).AddData(PagedResponse);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<PropertyRequestView>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PropertyRequestView> GetRequest(int Id)
        {
            try
            {
                var request = _propertyRequestRepository.GetRequests().AsQueryable()
                                    .ProjectTo<PropertyRequestView>(_mappingConfigurations)
                                    .AsEnumerable().FirstOrDefault(p => p.Id == Id);



                return StandardResponse<PropertyRequestView>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL).AddData(request);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PropertyRequestView>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PropertyRequestMatchView> AddMatch(int PropertyId, int RequestId)
        {
            try
            {
                var match = new PropertyRequestMatch()
                {
                    PropertyId = PropertyId,
                    PropertyRequestId = RequestId,
                    StatusId = (int)Statuses.PENDING
                };
                var result = _propertyRequestRepository.AddMatch(match);

                if (!result.Succeeded)
                    return StandardResponse<PropertyRequestMatchView>.Failed();

                return StandardResponse<PropertyRequestMatchView>.Ok().AddData(_mapper.Map<PropertyRequestMatchView>(result.Match));
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public StandardResponse<PropertyRequestMatchView> AcceptMatch(int matchId)
        {
            try
            {
                var thisMatch = _propertyRequestRepository.GetMatch(matchId);
                if (thisMatch == null)
                    return StandardResponse<PropertyRequestMatchView>.Error(StandardResponseMessages.ERROR_OCCURRED);
                
                thisMatch.StatusId = (int)Statuses.ACCEPTED;
                var result = _propertyRequestRepository.UpdateMatch(thisMatch);

                return StandardResponse<PropertyRequestMatchView>.Ok().AddData(_mapper.Map<PropertyRequestMatchView>(result));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PropertyRequestMatchView>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PropertyRequestMatchView> RejectMatch(int matchId)
        {
            try
            {
                var thisMatch = _propertyRequestRepository.GetMatch(matchId);
                if (thisMatch == null)
                    return StandardResponse<PropertyRequestMatchView>.Error(StandardResponseMessages.ERROR_OCCURRED);

                thisMatch.StatusId = (int)Statuses.REJECTED;
                var result = _propertyRequestRepository.UpdateMatch(thisMatch);

                return StandardResponse<PropertyRequestMatchView>.Ok().AddData(_mapper.Map<PropertyRequestMatchView>(result));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PropertyRequestMatchView>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        // public StandardResponse<PropertyRequestView> AcceptMatch(int PropertyId,int matchId)
        // {

        // }
    }
}