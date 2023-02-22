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
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly PagingOptions _defaultPagingOptions;

        public ReportController(IReportService reportService, IOptions<PagingOptions> defaultPagingOptions)
        {
            _reportService = reportService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        [HttpPost("create", Name = nameof(CreateReport))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<string>> CreateReport(ReportModel model)
        {
            return Ok(_reportService.CreateReport(model));
        }

        [HttpGet("list", Name = nameof(GetReports))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<ReportView>>> GetReports([FromQuery]PagingOptions pagingOptions)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_reportService.GetReports(pagingOptions));
        }

        [HttpGet("{id}", Name = nameof(GetReportById))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<ReportView>> GetReportById(int id)
        {
            return Ok(_reportService.GetReportById(id));
        }

        [HttpPost("contact", Name = nameof(ContactUs))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<string>> ContactUs(ReportModel model)
        {
            return Ok(_reportService.ContactUs(model));
        }
    }
}