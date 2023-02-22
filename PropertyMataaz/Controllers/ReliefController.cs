using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Services.Interfaces;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReliefController : ControllerBase
    {
        private readonly IReliefService _reliefService;

        public ReliefController(IReliefService reliefService)
        {
            _reliefService = reliefService;
        }

        [HttpGet("user", Name = nameof(ListMyRentReliefs))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<IEnumerable<RentReliefView>>> ListMyRentReliefs()
        {
            return Ok( _reliefService.ListMyReliefs());
        }

    }
}