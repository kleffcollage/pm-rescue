using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using PropertyMataaz.Controllers;
using PropertyMataaz.Models;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Services.Interfaces;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IConfigurationProvider _maapingConfiguration;

        public RequestService(IRequestRepository requestRepository, IConfigurationProvider maapingConfiguration)
        {
            _requestRepository = requestRepository;
            _maapingConfiguration = maapingConfiguration;
        }

        public StandardResponse<PagedCollection<RequestView>> ListOngoingRequests(PagingOptions pagingOptions)
        {
            var Requests = _requestRepository.ListOnGoingRequests()
                                            .AsQueryable().OrderByDescending(a => a.Id)
                                            .ProjectTo<RequestView>(_maapingConfiguration)
                                            .AsEnumerable();

            var PaggedRequests = Requests.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

            var PagedResponse = PagedCollection<RequestView>.Create(Link.ToCollection(nameof(RequestController.ListOngoingRequests)), PaggedRequests.ToArray(), Requests.Count(), pagingOptions);

            return StandardResponse<PagedCollection<RequestView>>.Ok().AddData(PagedResponse).AddStatusMessage(StandardResponseMessages.SUCCESSFUL);
        }

        public StandardResponse<PagedCollection<RequestView>> ListPendingRequests(PagingOptions pagingOptions)
        {
            var Requests = _requestRepository.ListPendingRequests()
                                            .AsQueryable().OrderByDescending(a => a.Id)
                                            .ProjectTo<RequestView>(_maapingConfiguration)
                                            .AsEnumerable();

            var PaggedRequests = Requests.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

            var PagedResponse = PagedCollection<RequestView>.Create(Link.ToCollection(nameof(RequestController.ListPendingRequests)), PaggedRequests.ToArray(), Requests.Count(), pagingOptions);

            return StandardResponse<PagedCollection<RequestView>>.Ok().AddData(PagedResponse).AddStatusMessage(StandardResponseMessages.SUCCESSFUL);
        }

        public StandardResponse<PagedCollection<RequestView>> ListRequests(PagingOptions pagingOptions)
        {
            var Requests = _requestRepository.ListRequests()
                                            .AsQueryable().OrderByDescending(a => a.Id)
                                            .ProjectTo<RequestView>(_maapingConfiguration)
                                            .AsEnumerable();

            var PaggedRequests = Requests.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

            var PagedResponse = PagedCollection<RequestView>.Create(Link.ToCollection(nameof(RequestController.ListRequests)), PaggedRequests.ToArray(), Requests.Count(), pagingOptions);

            return StandardResponse<PagedCollection<RequestView>>.Ok().AddData(PagedResponse).AddStatusMessage(StandardResponseMessages.SUCCESSFUL);
        }


        public StandardResponse<PagedCollection<RequestView>> ListResolvedRequests(PagingOptions pagingOptions)
        {
            var Requests = _requestRepository.ListResolvedRequests()
                                             .AsQueryable().OrderByDescending(a => a.Id)
                                             .ProjectTo<RequestView>(_maapingConfiguration)
                                             .AsEnumerable();

            var PaggedRequests = Requests.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

            var PagedResponse = PagedCollection<RequestView>.Create(Link.ToCollection(nameof(RequestController.ListResolvedRequests)), PaggedRequests.ToArray(), Requests.Count(), pagingOptions);

            return StandardResponse<PagedCollection<RequestView>>.Ok().AddData(PagedResponse).AddStatusMessage(StandardResponseMessages.SUCCESSFUL); ;
        }

        public StandardResponse<RequestView> GetRequest(int Id)
        {
            try
            {
                var request = _requestRepository.ListRequests()
                                                .AsQueryable().OrderByDescending(a => a.Id)
                                                .ProjectTo<RequestView>(_maapingConfiguration)
                                                .AsEnumerable().FirstOrDefault(r => r.Id == Id);

                return StandardResponse<RequestView>.Ok(request);

            }
            catch (Exception ex)
            {
                return StandardResponse<RequestView>.Failed();
            }
        }
    }
}