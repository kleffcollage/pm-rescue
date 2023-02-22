using System.Collections;
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
    public class ComplaintsController : ControllerBase
    {
        private readonly IComplaintsCategoryService _complaintsCategoryService;
        private readonly IComplaintsService _complaintsService;
        private readonly PagingOptions _defaultPagingOptions;

        public ComplaintsController(IComplaintsCategoryService complaintsCategoryService, IComplaintsService complaintsService,IOptions<PagingOptions> defaultPagingOptions)
        {
            _complaintsCategoryService = complaintsCategoryService;
            _defaultPagingOptions = defaultPagingOptions.Value;
            _complaintsService = complaintsService;
        }

        [HttpPost("create", Name = nameof(CreateComplaints))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<ComplaintsView>> CreateComplaints(ComplaintsModel model)
        {
            return Ok(_complaintsService.CreateComplaints(model));
        }

        [HttpGet("list", Name = nameof(ListMyComplaints))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<IEnumerable<ComplaintsView>>> ListMyComplaints([FromQuery]PagingOptions options)
        {
            options.Replace(_defaultPagingOptions);
            return Ok(_complaintsService.ListMyComplaints());
        }

        [HttpGet("property/{propertyId}/list", Name = nameof(ListComplaints))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<ComplaintsView>>> ListComplaints([FromQuery]PagingOptions options,int propertyId)
        {
            options.Replace(_defaultPagingOptions);
            return Ok(_complaintsService.ListComplaints(options,propertyId));
        }

        [HttpGet("authorize/{complaintsId}", Name = nameof(AuthorizeComplaints))]
        public ActionResult<StandardResponse<ComplaintsView>> AuthorizeComplaints(int complaintsId)
        {
            return Ok(_complaintsService.AuthorizeComplaints(complaintsId));
        }
        [HttpPost("categories/create", Name = nameof(CreateCategory))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<ComplaintsCategory>> CreateCategory(NameModel model)
        {
            return Ok(_complaintsCategoryService.CreateCategory(model));
        }

        [HttpPost("subcategory/create", Name = nameof(CreateSubcategory))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<ComplaintsSubCategory>> CreateSubcategory(ComplaintsSubCategory model)
        {
            return Ok(_complaintsCategoryService.CreateSubCategory(model));
        }

        [HttpGet("categories/list", Name = nameof(ListCategories))]
        public ActionResult<StandardResponse<IEnumerable<ComplaintsCategory>>> ListCategories()
        {
            return Ok(_complaintsCategoryService.ListCategories());
        }

    }
}