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
    [Authorize(Roles = "ADMIN")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;
        private readonly PagingOptions _defaultPagingOptions;
        private IPropertyRequestService _propertyRequestService;
        public ICleaningService _cleanService;
        public readonly ILandSearchService _landService;
        private readonly IPaymentService _paymentService;
        private readonly IApplicationService _applicationService;
        private readonly ITenancyService _tenancyService;
        private readonly IRequestService _requestService;

        public AdminController(IUserService userService, IOptions<PagingOptions> defaultPagingOptions, IPropertyService propertyService, IPropertyRequestService propertyRequestService, ICleaningService cleanService, IPaymentService paymentService, IApplicationService applicationService, ITenancyService tenancyService, IRequestService requestService)
        {
            _userService = userService;
            _defaultPagingOptions = defaultPagingOptions.Value;
            _propertyService = propertyService;
            _propertyRequestService = propertyRequestService;
            _cleanService = cleanService;
            _paymentService = paymentService;
            _applicationService = applicationService;
            _tenancyService = tenancyService;
            _requestService = requestService;
        }

        [HttpPost("token", Name = nameof(Authenticate))]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public ActionResult<StandardResponse<UserView>> Authenticate(LoginModel userToLogin)
        {
            return Ok(_userService.AuthenticateAdmin(userToLogin));
        }

        [HttpPost("create", Name = nameof(Create))]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public ActionResult<StandardResponse<UserView>> Create(Register newUser)
        {
            return Ok(_userService.CreateAdminUser(newUser));
        }

        [HttpGet("list", Name = nameof(ListAdmins))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<UserView>>> ListAdmins([FromQuery] PagingOptions pagingOptions = null, [FromQuery] string search = null)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_userService.ListAllAAdminUsers(pagingOptions, search));
        }

        [HttpGet("user/{userId}", Name = nameof(GetUser))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<UserView>> GetUser(int userId)
        {
            return Ok(_userService.GetUser(userId));
        }

        [HttpGet("metrics", Name = nameof(Metrics))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<MetricsView>> Metrics()
        {
            return Ok(_userService.Metrics());
        }

        [HttpGet("delete/{email}", Name = nameof(DeleteUser))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<UserView>> DeleteUser(string email)
        {
            return Ok(_userService.DeleteUser(email));
        }

        [HttpPost("property/create", Name = nameof(CreateNewProperty))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PropertyView>> CreateNewProperty(PropertyModel model)
        {
            return Ok(_propertyService.CreatePropertyAdmin(model));
        }

        [HttpGet("requests/list", Name = nameof(ListAllRequests))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<PropertyRequestView>>> ListAllRequests([FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_propertyRequestService.GetRequests(pagingOptions));
        }

        [HttpGet("requests/get/{Id}", Name = nameof(GetRequest))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PropertyRequestView>> GetRequest(int Id)
        {
            return Ok(_propertyRequestService.GetRequest(Id));
        }

        [HttpGet("properties/list", Name = nameof(ListProperties))]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public ActionResult<StandardResponse<PropertyView>> ListProperties([FromQuery] PagingOptions pagingOptions, [FromQuery] string search, [FromQuery] string filter)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_propertyService.ListAllProperties(pagingOptions, search, filter));
        }

        [HttpGet("properties/rent/list", Name = nameof(ListRentProperties))]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public ActionResult<StandardResponse<PropertyView>> ListRentProperties([FromQuery] PagingOptions pagingOptions, [FromQuery] string search, [FromQuery] string filter)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_propertyService.ListAllRentProperties(pagingOptions, search, filter));
        }

        [HttpGet("properties/sale/list", Name = nameof(ListSaleProperties))]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public ActionResult<StandardResponse<PropertyView>> ListSaleProperties([FromQuery] PagingOptions pagingOptions, [FromQuery] string search, [FromQuery] string filter)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_propertyService.ListAllSaleProperties(pagingOptions, search, filter));
        }

        [HttpGet("properties/list/rent/pending", Name = nameof(ListPropertiesForRentPending))]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public ActionResult<StandardResponse<PropertyView>> ListPropertiesForRentPending([FromQuery] PagingOptions pagingOptions, [FromQuery] string search)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_propertyService.ListAllPropertiesForRentReview(pagingOptions, search));
        }

        [HttpGet("properties/list/rent", Name = nameof(ListPropertiesRentApproved))]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public ActionResult<StandardResponse<PropertyView>> ListPropertiesRentApproved([FromQuery] PagingOptions pagingOptions, [FromQuery] string search, [FromQuery] string filter)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_propertyService.ListAllPropertiesRentApproved(pagingOptions, search, filter));
        }

        [HttpGet("property/approve/{propertyId}", Name = nameof(ApproveProperty))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PropertyView>> ApproveProperty(int propertyId)
        {
            return Ok(_propertyService.Approve(propertyId));
        }

        [HttpGet("property/reject/{propertyId}/{reason}", Name = nameof(RejectProperty))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PropertyView>> RejectProperty(int propertyId, string reason)
        {
            return Ok(_propertyService.Reject(propertyId, reason));
        }
        [HttpGet("clean/requests/list", Name = nameof(ListAllCleanRequests))]
        [ProducesResponseType(200)]
        public ActionResult<PagedCollection<CleaningView>> ListAllCleanRequests([FromQuery] PagingOptions options)
        {
            options.Replace(_defaultPagingOptions);
            return Ok(_cleanService.ListAllRequests(options));
        }

        [HttpGet("clean/requests/get/{Id}", Name = nameof(GetCleanRequest))]
        [ProducesResponseType(200)]
        public ActionResult<CleaningView> GetCleanRequest(int Id)
        {
            return Ok(_cleanService.GetRequestById(Id));
        }

        [HttpGet("land/requests/list", Name = nameof(ListAllLandRequests))]
        [ProducesResponseType(200)]
        public ActionResult<PagedCollection<CleaningView>> ListAllLandRequests([FromQuery] PagingOptions options)
        {
            options.Replace(_defaultPagingOptions);
            return Ok(_landService.ListRequests(options));
        }

        [HttpPost("clean/quote", Name = nameof(AddQuoteForCleanRequest))]
        public ActionResult<StandardResponse<CleaningQuoteView>> AddQuoteForCleanRequest(CleaningQuoteModel model)
        {
            return Ok(_cleanService.AddQuoteToRequest(model));
        }

        [HttpGet("transactions/list", Name = nameof(ListTransactions))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<Transaction>>> ListTransactions([FromQuery] PagingOptions pagingOptions, [FromQuery] string search)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_paymentService.ListTransactions(pagingOptions, search));
        }

        [HttpGet("enquiries/list", Name = nameof(ListAllEnquiries))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<UserEnquiryView>>> ListAllEnquiries([FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_userService.ListAllEnquiries(pagingOptions));
        }

        [HttpGet("enquiries/get/{enquiryId}", Name = nameof(GetEnquiry))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<UserEnquiryView>> GetEnquiry(int enquiryId)
        {
            return Ok(_userService.GetEnquiryById(enquiryId));
        }

        [HttpGet("applications/reliefs/pending", Name = nameof(ListReliefApplications))]
        public ActionResult<StandardResponse<PagedCollection<ApplicationView>>> ListReliefApplications([FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_applicationService.ListPendingReliefApplications(pagingOptions));
        }

        [HttpGet("applications/reliefs/accepted", Name = nameof(ListAcceptedReliefApplications))]
        public ActionResult<StandardResponse<PagedCollection<ApplicationView>>> ListAcceptedReliefApplications([FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_applicationService.ListAcceptedReliefApplications(pagingOptions));
        }

        [HttpGet("applications/reliefs/reviewed", Name = nameof(ListReviewedReliefApplications))]
        public ActionResult<StandardResponse<PagedCollection<ApplicationView>>> ListReviewedReliefApplications([FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_applicationService.ListReviewedReliefApplications(pagingOptions));
        }

        [HttpGet("applications/reliefs/approved", Name = nameof(ListApprovedReliefApplications))]
        public ActionResult<StandardResponse<PagedCollection<ApplicationView>>> ListApprovedReliefApplications([FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_applicationService.ListApprovedReliefApplications(pagingOptions));
        }

        [HttpGet("applications/rent", Name = nameof(ListRentApplications))]
        public ActionResult<StandardResponse<PagedCollection<ApplicationView>>> ListRentApplications([FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_applicationService.ListRentApplications(pagingOptions));
        }

        [HttpGet("tenancies", Name = nameof(ListTenancies))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<TenancyView>>> ListTenancies([FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_tenancyService.ListAllTenancies(pagingOptions));
        }

        [HttpGet("enquiries/user/get/{id}", Name = nameof(ListUserEnquiries))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<UserEnquiry>>> ListUserEnquiries( int id,[FromQuery] PagingOptions pagingOptions = null)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_userService.ListUserEnquiriesAdmin(pagingOptions, id));
        }

        [HttpGet("list/user/{id}", Name = nameof(ListUsersRequestsAdmin))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<PropertyRequestView>>> ListUsersRequestsAdmin([FromQuery] PagingOptions pagingOptions, int id)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_propertyRequestService.GetUsersRequestsAdmin(pagingOptions, id));
        }

    }
}