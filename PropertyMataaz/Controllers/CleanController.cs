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
    // [Authorize]
    public class CleanController : ControllerBase
    {
        private readonly ICleaningService _cleanService;
        private readonly PagingOptions _defaultPagingOptions;
        public CleanController(ICleaningService cleanService, IOptions<PagingOptions> defaultPagingOptions)
        {
            _cleanService = cleanService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        [HttpPost("request", Name = nameof(CreateNewRequest))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<CleaningView>> CreateNewRequest(CleaningModel model)
        {
            return Ok(_cleanService.CreateRequest(model));
        }

        [HttpGet("requests/user", Name = nameof(ListMyRequests))]
        [ProducesResponseType(200)]
        public ActionResult<PagedCollection<CleaningView>> ListMyRequests([FromQuery] PagingOptions options)
        {
            options.Replace(_defaultPagingOptions);
            return Ok(_cleanService.ListMyRequests(options));
        }

        [HttpGet("quote/accept/{id}", Name = nameof(AcceptQuote))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<CleaningView>> AcceptQuote(int id)
        {
            return Ok(_cleanService.AcceptQuote(id));
        }

        [HttpGet("quote/reject/{id}", Name = nameof(RejectQuote))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<CleaningView>> RejectQuote(int id)
        {
            return Ok(_cleanService.RejectQuote(id));
        }

    }
}