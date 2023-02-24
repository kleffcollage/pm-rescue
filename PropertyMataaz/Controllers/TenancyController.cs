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
    public class TenancyController : ControllerBase
    {
        private readonly ITenancyService _tenancyService;
        public TenancyController(ITenancyService tenancyService)
        {
            _tenancyService = tenancyService;
        }

        [HttpGet("user", Name = nameof(ListMyTenancies))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<IEnumerable<TenancyView>>> ListMyTenancies()
        {
            return Ok(_tenancyService.ListMyTenancies());
        }



        [HttpGet("landlord", Name = nameof(ListMyTenants))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<IEnumerable<TenancyView>>> ListMyTenants()
        {
            return Ok(_tenancyService.ListMyTenants());
        }

        [HttpGet("agreemet/{tenancyId}", Name = nameof(GetTenancyAgreement))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<string>> GetTenancyAgreement(int tenancyId)
        {
            return Ok(_tenancyService.GetAgreement(tenancyId));
        }

        [HttpPost("renewable/toggle/{id}", Name = nameof(ToggleRenewability))]
        public ActionResult<StandardResponse<TenancyView>> ToggleRenewability(int id)
        {
            return Ok(_tenancyService.ToggleRenewability(id));
        }

        [HttpPost("agreement/update/{id}", Name = nameof(UpdateTenancyAgreement))]
        public ActionResult<StandardResponse<bool>> UpdateTenancyAgreement(int id)
        {
            return Ok(_tenancyService.UpdateTenancyAgreement(id));
        }
    }
}