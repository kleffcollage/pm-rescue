using Google.Apis.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Services.Interfaces;

namespace PropertyMataaz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LandSearchController : ControllerBase
    {
        public readonly ILandSearchService _landService;
        public readonly PagingOptions _defaultPagingOptions;

        public LandSearchController(ILandSearchService landService, IOptions<PagingOptions> defaultPagingOptions)
        {
            _landService = landService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        [HttpPost("create", Name = nameof(CreateRequest))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<LandSearchView>> CreateRequest(LandSearchModel model)
        {
            return Ok(_landService.CreateRequest(model));
        }

        [HttpGet("user/list", Name = nameof(ListMyLandRequests))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<LandSearchView>>> ListMyLandRequests([FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_landService.ListMyRequests(pagingOptions));
        }
    }
}