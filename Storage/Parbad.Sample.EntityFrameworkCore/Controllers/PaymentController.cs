﻿using Microsoft.AspNetCore.Mvc;
using Parbad.AspNetCore;
using Parbad.Sample.Shared;
using System.Threading.Tasks;

namespace Parbad.Sample.EntityFrameworkCore.Controllers;

public class PaymentController : Controller
{
    private readonly IOnlinePayment _onlinePayment;

    public PaymentController(IOnlinePayment onlinePayment)
    {
        _onlinePayment = onlinePayment;
    }

    [HttpGet]
    public IActionResult Pay()
    {
        return View(new PayViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Pay(PayViewModel viewModel)
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

        // Note: Save the result.TrackingNumber in your database.

        if (result.IsSucceed)
        {
            return result.GatewayTransporter.TransportToGateway();
        }

        return View("PayRequestError", result);
    }

    [HttpGet, HttpPost]
    public async Task<IActionResult> Verify()
    {
        var invoice = await _onlinePayment.FetchAsync();

        // Check if the invoice is new or it's already processed before.
        if (invoice.Status == PaymentFetchResultStatus.AlreadyProcessed)
        {
            // You can also see if the invoice is already verified before.
            var isAlreadyVerified = invoice.IsAlreadyVerified;

            return Content("The payment is already processed before.");
        }

        // This is an example of cancelling an invoice when you think that the payment process must be stopped.
        if (!Is_There_Still_Product_In_Shop(invoice.TrackingNumber))
        {
            var cancelResult = await _onlinePayment.CancelAsync(invoice, cancellationReason: "Sorry, We have no more products to sell.");

            return View("CancelResult", cancelResult);
        }

        var verifyResult = await _onlinePayment.VerifyAsync(invoice);

        // Note: Save the verifyResult.TransactionCode in your database.

        return View(verifyResult);
    }

    [HttpGet]
    public IActionResult Refund()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Refund(RefundViewModel viewModel)
    {
        var result = await _onlinePayment.RefundCompletelyAsync(viewModel.TrackingNumber);

        return View("RefundResult", result);
    }

    private static bool Is_There_Still_Product_In_Shop(long trackingNumber)
    {
        // Yes, we still have products :)

        return true;
    }
}
