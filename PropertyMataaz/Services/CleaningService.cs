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
    public class CleaningService : ICleaningService
    {
        private readonly ICleanRepository _cleaningRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IConfigurationProvider _mappingConfigurations;
        public CleaningService(ICleanRepository cleaningRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfigurationProvider mappingConfigurations)
        {
            _cleaningRepository = cleaningRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _mappingConfigurations = mappingConfigurations;
        }

        public StandardResponse<CleaningView> AcceptQuote(int Id)
        {
            try
            {
                var thisQuote = _cleaningRepository.GetQuoteById(Id);
                thisQuote.StatusId = (int)Statuses.APPROVED;
                thisQuote = _cleaningRepository.UpdateQuote(thisQuote);
                return StandardResponse<CleaningView>.Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<CleaningView>.Failed();
            }
        }

        public StandardResponse<CleaningQuoteView> AddQuoteToRequest(CleaningQuoteModel cleaningQuote)
        {
            try
            {
                var mappedQuote = _mapper.Map<CleaningQuote>(cleaningQuote);
                var thisCleaning = _cleaningRepository.ListCleaning().FirstOrDefault(c => c.Id == cleaningQuote.CleaningId);
                mappedQuote.StatusId = (int)Statuses.PENDING;
                var createdQuote = _cleaningRepository.CreateCleaningQuote(mappedQuote);
                var updatedCleaning = _cleaningRepository.Update(thisCleaning);
                return StandardResponse<CleaningQuoteView>.Ok(_mapper.Map<CleaningQuoteView>(createdQuote));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<CleaningQuoteView>.Failed();
            }
        }

        public StandardResponse<CleaningView> CreateRequest(CleaningModel cleaningModel)
        {
            try
            {
                int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
                var mappedRequest = _mapper.Map<Cleaning>(cleaningModel);
                mappedRequest.UserId = UserId;
                mappedRequest.StatusId = (int)Statuses.ACTIVE;
                var createdRequest = _cleaningRepository.CreateAndReturn(mappedRequest);

                return StandardResponse<CleaningView>.Ok(_mapper.Map<CleaningView>(createdRequest));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<CleaningView>.Failed();
            }
        }

        public StandardResponse<PagedCollection<CleaningView>> ListAllRequests(PagingOptions options)
        {
            try
            {
                var requests = _cleaningRepository.ListCleaning().Where(c => c.StatusId == (int)Statuses.ACTIVE)
                                .AsQueryable().OrderByDescending(a => a.Id)
                                .ProjectTo<CleaningView>(_mappingConfigurations)
                                .AsEnumerable();
                var pagedRequests = requests.Skip(options.Offset.Value).Take(options.Limit.Value);

                var pagedCollection = PagedCollection<CleaningView>.Create(Link.ToCollection(nameof(AdminController.ListAllCleanRequests)), pagedRequests.ToArray(), requests.Count(), options);
                return StandardResponse<PagedCollection<CleaningView>>.Ok(pagedCollection);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<CleaningView>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<CleaningView> GetRequestById(int Id)
        {
            try
            {
                var requests = _cleaningRepository.ListCleaning().Where(c => c.Id == Id).FirstOrDefault();
                return StandardResponse<CleaningView>.Ok(_mapper.Map<CleaningView>(requests));

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<CleaningView>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PagedCollection<CleaningView>> ListMyRequests(PagingOptions options)
        {
            try
            {
                int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();

                var requests = _cleaningRepository.ListCleaning().Where(c => c.StatusId == (int)Statuses.ACTIVE && c.UserId == UserId)
                                .AsQueryable().OrderByDescending(a => a.Id)
                                .ProjectTo<CleaningView>(_mappingConfigurations)
                                .AsEnumerable();
                var pagedRequests = requests.Take(options.Limit.Value).Skip(options.Offset.Value);

                var pagedCollection = PagedCollection<CleaningView>.Create(Link.ToCollection(nameof(CleanController.ListMyRequests)), pagedRequests.ToArray(), requests.Count(), options);
                return StandardResponse<PagedCollection<CleaningView>>.Ok(pagedCollection);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<CleaningView>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<CleaningView> RejectQuote(int Id)
        {
            try
            {
                var thisQuote = _cleaningRepository.GetQuoteById(Id);
                thisQuote.StatusId = (int)Statuses.REJECTED;
                thisQuote = _cleaningRepository.UpdateQuote(thisQuote);
                return StandardResponse<CleaningView>.Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<CleaningView>.Failed();
            }
        }
    }
}