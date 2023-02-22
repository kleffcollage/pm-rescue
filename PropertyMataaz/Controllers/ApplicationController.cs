using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PropertyMataaz.Models.AppModels;
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
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly PagingOptions _defaultPagingOptions;

        public ApplicationController(IApplicationService applicationService, IOptions<PagingOptions> defaultPagingOptions)
        {
            _applicationService = applicationService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        [HttpPost("new", Name = nameof(CreateApplication))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<ApplicationView>> CreateApplication(ApplicationModel model)
        {
            return Ok(_applicationService.CreateApplication(model));
        }

        [HttpGet("list/{propertyId}", Name = nameof(ListActiveApplications))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<ApplicationView>>> ListActiveApplications(int propertyId, [FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_applicationService.ListByProperty(propertyId, pagingOptions));
        }

        [HttpPost("get/{id}", Name = nameof(GetById))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<ApplicationView>> GetById(int id)
        {
            return Ok(_applicationService.GetById(id));
        }

        [HttpGet("approve/{id}", Name = nameof(Approve))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<ApplicationView>> Approve(int id)
        {
            return Ok(_applicationService.Approve(id));
        }

        [HttpGet("reject/{id}", Name = nameof(Reject))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<ApplicationView>> Reject(int id)
        {
            return Ok(_applicationService.Reject(id));
        }

        [HttpGet("accept/{id}", Name = nameof(Accept))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<ApplicationView>> Accept(int id)
        {
            return Ok(_applicationService.AcceptReliefApplication(id));
        }
        [HttpGet("review/{id}", Name = nameof(Review))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<ApplicationView>> Review(int id)
        {
            return Ok(_applicationService.ReviewReliefApplication(id));
        }

        [HttpGet("types", Name = nameof(ListApplicationTypes))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<IEnumerable<ApplicationType>>> ListApplicationTypes()
        {
            return Ok(_applicationService.ListTypes());
        }

        [HttpGet("get/user/property/{propertyId}", Name = nameof(GetUserApplication))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<ApplicationStatusView>> GetUserApplication(int propertyId)
        {
            return Ok(_applicationService.GetApplication(propertyId));
        }
    }
}