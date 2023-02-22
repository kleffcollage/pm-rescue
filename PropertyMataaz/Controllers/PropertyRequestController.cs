using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Services.Interfaces;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PropertyRequestController : ControllerBase
    {
        private IPropertyRequestService _propertyRequestService;
        private readonly PagingOptions _defaultPagingOptions;
        public PropertyRequestController(IPropertyRequestService propertyRequestService, IOptions<PagingOptions> defaultPagingOptions)
        {
            _propertyRequestService = propertyRequestService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }
        [HttpPost("new", Name = nameof(NewRequest))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PropertyView>> NewRequest(PropertyRequestInput request)
        {
            return Ok(_propertyRequestService.CreateRequest(request));
        }

        [HttpGet("list/user", Name = nameof(ListUsersRequests))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<PropertyRequestView>>> ListUsersRequests([FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_propertyRequestService.GetUsersRequests(pagingOptions));
        }

        [HttpGet("match/add/{propertyId}/{requestId}", Name = nameof(AddMatch))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PropertyRequestMatchView>> AddMatch(int propertyId, int requestId)
        {
            return Ok(_propertyRequestService.AddMatch(propertyId, requestId));
        }
        [HttpGet("match/remove/{matchId}", Name = nameof(RemoveMatch))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PropertyRequestMatchView>> RemoveMatch(int matchId)
        {
            return Ok(_propertyRequestService.RemoveMatch(matchId));
        }

        [HttpGet("match/accept/{matchId}", Name = nameof(AcceptMatch))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PropertyRequestMatchView>> AcceptMatch(int matchId)
        {
            return Ok(_propertyRequestService.AcceptMatch(matchId));
        }

        [HttpGet("match/reject/{matchId}", Name = nameof(RejectMatch))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PropertyRequestMatchView>> RejectMatch(int matchId)
        {
            return Ok(_propertyRequestService.RejectMatch(matchId));
        } 
    }
}
