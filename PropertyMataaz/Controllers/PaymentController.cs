using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Services.Interfaces;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("initiate", Name = nameof(InitiatePayment))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<string>> InitiatePayment(PaymentModel model)
        {
            return Ok(_paymentService.InitiatePayment(model));
        }

        [HttpGet("validate/{reference}/{transactionId}")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PaymentView>> ValidatePayment(string reference,int transactionId)
        {
            return Ok(_paymentService.ValidatePayment(reference,transactionId));
        }

        [HttpGet("rates/{propertyId}", Name = nameof(GetRatesForProperty))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PaymentRatesView>> GetRatesForProperty(int propertyId)
        {
            return Ok(_paymentService.GetPaymentRates(propertyId));
        }

        // [HttpGet("test/pdf", Name = nameof(TestPDF))]
        // [ProducesResponseType(200)]
        // [AllowAnonymous]
        // public ActionResult<StandardResponse<PaymentRatesView>> TestPDF()
        // {
        //     return Ok(_paymentService.GenerateTenancyAgreement());
        // }
    }
}