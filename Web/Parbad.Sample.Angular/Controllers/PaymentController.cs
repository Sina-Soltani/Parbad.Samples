using Microsoft.AspNetCore.Mvc;
using Parbad.Sample.Angular.Repositories;
using Parbad.Sample.Shared;
using System.Threading.Tasks;

namespace Parbad.Sample.Angular.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IOnlinePayment _onlinePayment;
        private readonly IOrderRepository _orderRepository;

        public PaymentController(IOnlinePayment onlinePayment, IOrderRepository orderRepository)
        {
            _onlinePayment = onlinePayment;
            _orderRepository = orderRepository;
        }

        [HttpPost("pay")]
        public async Task<IActionResult> Pay([FromBody] PayViewModel viewModel)
        {
            var callbackUrl = Url.Action("Verify", "Payment", null, Request.Scheme);

            var result = await _onlinePayment.RequestAsync(invoice =>
            {
                invoice
                    .SetAmount(viewModel.Amount)
                    .SetCallbackUrl(callbackUrl)
                    .SetGateway(viewModel.SelectedGateway.ToString());

                if (viewModel.GenerateTrackingNumberAutomatically)
                {
                    invoice.UseAutoIncrementTrackingNumber();
                }
                else
                {
                    invoice.SetTrackingNumber(viewModel.TrackingNumber);
                }
            });

            _orderRepository.AddOrder(new Order
            {
                TrackingNumber = result.TrackingNumber,
                Amount = result.Amount,
                GatewayName = result.GatewayName,
                GatewayAccountName = result.GatewayAccountName
            });

            return Ok(result);
        }

        [Route("verify")]
        [HttpGet, HttpPost]
        public async Task<IActionResult> Verify()
        {
            var invoice = await _onlinePayment.FetchAsync();

            var clientAppUrl = "http://localhost:5000/payment-result";

            if (invoice.Status == PaymentFetchResultStatus.ReadyForVerifying)
            {
                var verifyResult = await _onlinePayment.VerifyAsync(invoice);

                _orderRepository.UpdateOrder(verifyResult.TrackingNumber, verifyResult.IsSucceed, verifyResult.Message, verifyResult.TransactionCode);
            }
            else
            {
                _orderRepository.OrderFailed(invoice.TrackingNumber, invoice.Message);
            }

            return Redirect(clientAppUrl);
        }
    }
}
