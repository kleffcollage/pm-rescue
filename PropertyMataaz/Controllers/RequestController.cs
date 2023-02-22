using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Services.Interfaces;
using PropertyMataaz.Utilities;
//using PropertyMataaz.Models;

namespace PropertyMataaz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private IRequestService _requestService;
        private readonly PagingOptions _defaultPagingOptions;
        public RequestController(IRequestService requestService, IOptions<PagingOptions> defaultPagingOptions)
        {
            _requestService = requestService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        [HttpGet("list", Name = nameof(ListRequests))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<RequestView>>> ListRequests([FromQuery]PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_requestService.ListRequests(pagingOptions));
        }
        
        [HttpGet("list/pending", Name = nameof(ListPendingRequests))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<RequestView>>> ListPendingRequests([FromQuery]PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_requestService.ListPendingRequests(pagingOptions));
        }

        [HttpGet("list/ongoing", Name = nameof(ListOngoingRequests))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<RequestView>>> ListOngoingRequests([FromQuery]PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_requestService.ListOngoingRequests(pagingOptions));
        }

        [HttpGet("list/resolved", Name = nameof(ListResolvedRequests))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<RequestView>>> ListResolvedRequests([FromQuery]PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_requestService.ListResolvedRequests(pagingOptions));
        }
    }
}