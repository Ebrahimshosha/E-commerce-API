using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using TalabatAPIs.Error;
using TalabatCore.Entities;
using TalabatCore.Repositories;
using TalabatServices;

namespace TalabatAPIs.Controllers
{
    public class PaymentController : APIBaseController
    {
        // This is your Stripe CLI webhook secret for testing your endpoint locally.
        const string endpointSecret = "whsec_585e394a15dd3c458c192140c31e5aaa83c4510a525fcd791d02057cfdd54bff";

        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiRespponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var Basket = await _paymentService.CrreateOrUpdatePaymentIntent(basketId);
            if (Basket is null) return BadRequest(new ApiRespponse(400, "Ther is a problem with your Basket"));
            return Ok(Basket); 
        }

        [HttpPost("webhook")] // BaseUrl/api/Payment/webhook
        public async Task<IActionResult> StripeWebHook()
        {

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], endpointSecret);

            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

            // Handle the event
            if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
            {
                await _paymentService.UpdaePaymentIntentWithSucceedorFailed(paymentIntent.Id, false);
                _logger.LogInformation("Payment is Failed :(", paymentIntent.Id);
            }
            else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            {
                await _paymentService.UpdaePaymentIntentWithSucceedorFailed(paymentIntent.Id, true);
                _logger.LogInformation("Payment is succeed :)", paymentIntent.Id);
            }
            // ... handle other event types
            else
            {
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }

            return Ok();

            // We Don't need to do Try and Catch because we alredy made My Own Exception MiddleWare
        }
    }
}
