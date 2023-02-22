using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Services.Interfaces;
using PropertyMataaz.Utilities;
//using PropertyMataaz.Models;

namespace PropertyMataaz.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        private readonly IUserService _userService;
        private readonly PagingOptions _defaultPagingOptions;
        public PropertyController(IPropertyService propertyService, IOptions<PagingOptions> defaultPagingOptions, IUserService userService)
        {
            _propertyService = propertyService;
            _defaultPagingOptions = defaultPagingOptions.Value;
            _userService = userService;
        }

        [HttpPost("create", Name = nameof(CreateProperty))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PropertyView>> CreateProperty(PropertyModel model)
        {
            return Ok(_propertyService.CreateProperty(model));
        }

        [HttpGet("list", Name = nameof(ListAllProperties))]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public ActionResult<StandardResponse<PropertyView>> ListAllProperties([FromQuery] PagingOptions pagingOptions, [FromQuery] string search, [FromQuery] string filter)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_propertyService.ListAllProperties(pagingOptions, search, filter));
        }

        [HttpGet("list/sales", Name = nameof(ListAllPropertiesForSale))]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public ActionResult<StandardResponse<PropertyView>> ListAllPropertiesForSale([FromQuery] PagingOptions pagingOptions, [FromQuery] string search, [FromQuery] PropertyFilterOptions filterOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_propertyService.PropertyForSales(pagingOptions, search, filterOptions));
        }

        [HttpGet("list/rent", Name = nameof(ListAllPropertiesForRent))]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public ActionResult<StandardResponse<PropertyView>> ListAllPropertiesForRent([FromQuery] PagingOptions pagingOptions, [FromQuery] string search, [FromQuery] PropertyFilterOptions filterOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_propertyService.PropertyForRent(pagingOptions, search, filterOptions));
        }

        [HttpGet("get/{Id}", Name = nameof(Get))]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public ActionResult<StandardResponse<PropertyView>> Get(int Id)
        {
            return Ok(_propertyService.GetPropertyById(Id));
        }

        [HttpGet("types", Name = nameof(GetAllTypes))]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public ActionResult<StandardResponse<PropertyView>> GetAllTypes()
        {
            return Ok(_propertyService.GetAllTypes());
        }

        [HttpGet("titles", Name = nameof(ListTitleTypes))]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public ActionResult<StandardResponse<IEnumerable<PropertyTitle>>> ListTitleTypes()
        {
            return Ok(_propertyService.GetTitleTypes());
        }

        [HttpGet("user/created", Name = nameof(ListMyProperties))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<PropertyView>>> ListMyProperties([FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_propertyService.ListUsersAddedProperties(pagingOptions));
        }

        [HttpGet("user/created/sale", Name = nameof(ListMyPropertiesForSale))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<PropertyView>>> ListMyPropertiesForSale([FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_propertyService.ListUsersAddedPropertiesForSale(pagingOptions));
        }

        [HttpGet("user/created/rent", Name = nameof(ListMyPropertiesForRent))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PropertyView>> ListMyPropertiesForRent([FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_propertyService.ListUsersAddedPropertiesForRent(pagingOptions));
        }

        [HttpGet("user/drafts", Name = nameof(ListMyDrafts))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PropertyView>> ListMyDrafts([FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_propertyService.ListUserDrafts(pagingOptions));
        }

        [HttpGet("delete/{id}", Name = nameof(DeleteProperty))]
        [ProducesResponseType(200)]
        public ActionResult DeleteProperty(int id)
        {
            return Ok(_propertyService.Delete(id));
        }

        [HttpGet("deactivate/{id}", Name = nameof(DeActivate))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<bool>> DeActivate(int id)
        {
            return Ok(_propertyService.Deactivate(id));
        }

        [AllowAnonymous]
        [HttpGet("addview/{Id}", Name = nameof(IncrementViews))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PropertyView>> IncrementViews(int Id)
        {
            return Ok(_propertyService.IncrementViews(Id));
        }

        [AllowAnonymous]
        [HttpGet("addenquiries/{Id}", Name = nameof(IncrementEnquiries))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PropertyView>> IncrementEnquiries(int Id)
        {
            return Ok(_userService.AddEnquiry(Id));
        }

        [AllowAnonymous]
        [HttpPost("inspectiondates/create", Name = nameof(CreateInspectionDate))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<InspectionDateView>> CreateInspectionDate(InspectionDateModel newDate)
        {
            return Ok(_propertyService.CreateDate(newDate));
        }

        [HttpGet("inspectiondates/delete/{id}", Name = nameof(DeleteInspectionDate))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<bool>> DeleteInspectionDate(int id)
        {
            return Ok(_propertyService.DeleteDate(id));
        }

        [AllowAnonymous]
        [HttpPost("inspectiontime/create", Name = nameof(CreateInspectionTime))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<InspectionDateView>> CreateInspectionTime(InspectionTimeModel newTime)
        {
            return Ok(_propertyService.CreateTime(newTime));
        }

        [AllowAnonymous]
        [HttpGet("inspectiondates/list/{PropertyId}", Name = nameof(ListInspectionDates))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<IEnumerable<InspectionDateView>>> ListInspectionDates(int PropertyId)
        {
            return Ok(_propertyService.GetInspectionDates(PropertyId));
        }

        [HttpPost("update", Name = nameof(UpdateProperty))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PropertyView>> UpdateProperty(PropertyModel model)
        {
            return _propertyService.UpdateProperty(model);
        }

        [HttpGet("tenants/types", Name = nameof(ListTenantTypes))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<IEnumerable<TenantType>>> ListTenantTypes()
        {
            return Ok(_propertyService.ListTenantTypes());
        }

        [HttpGet("collection/types", Name = nameof(ListRentCollectionType))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<IEnumerable<RentCollectionType>>> ListRentCollectionType()
        {
            return Ok(_propertyService.ListRentCollectionType());
        }

        [HttpPost("inspections/schedule", Name = nameof(ScheduleInspection))]
        public ActionResult<StandardResponse<bool>> ScheduleInspection(InspectionModel model)
        {
            return Ok(_propertyService.ScheduleInspection(model));
        }

        [HttpPost("enquiry/cancel/{propertyId}", Name = nameof(CancelEnquiry))]
        public ActionResult<StandardResponse<bool>> CancelEnquiry(int propertyId)
        {
            return Ok(_propertyService.CancelEnquiry(propertyId));
        }

        [HttpGet("inspections/user/property/{propertyId}", Name = nameof(GetInspection))]
        public ActionResult<StandardResponse<InspectionView>> GetInspection(int propertyId)
        {
            return Ok(_propertyService.GetUsersInspectionForProperty(propertyId));
        }

        [HttpGet("property/receipt/{propertyId}", Name = nameof(GetReceipt))]

        public ActionResult<StandardResponse<ReceiptView>> GetReceipt(int PropertyId)
        {
            return _propertyService.GetPaymentReceipt(PropertyId);
        }
        [HttpGet("property/agreement/{propertyId}", Name = nameof(GetAgreement))]

        public ActionResult<StandardResponse<string>> GetAgreement(int PropertyId)
        {
            return StandardResponse<string>.Ok("http://www.africau.edu/images/default/sample.pdf");
        }

    }
}

