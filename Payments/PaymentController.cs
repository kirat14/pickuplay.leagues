using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pickuplay.DTOs;
using Pickuplay.Enums;
using Pickuplay.Teams.Payments.DTO;
using Pickuplay.Teams.Payments.Options;
using Stripe;

namespace Pickuplay.Teams.Payments;

[ApiController]
[Route("api/payments")]
public class PaymentsController(PaymentService paymentService, ILogger<PaymentsController> logger)
    : ControllerBase
{
    [HttpPost("create-intent")]
    [Authorize]
    public async Task<IActionResult> CreateIntent(
        [FromBody] CreateIntentRequest request,
        CancellationToken ct
    )
    {
        logger.LogInformation("Here");
        var userId = User.FindFirst("id")?.Value;

        if (!int.TryParse(userId, out var playerId))
        {
            throw new UnauthorizedAccessException("User ID could not be found in the token.");
        }

        long.TryParse(userId, out long userIdLong);
        var clientSecret = await paymentService.CreateIntentAsync(
            userIdLong,
            request.ItemType,
            request.ItemId,
            request.AmountInCents,
            ct
        );

        return Ok(
            new ApiResponse<object>(
                ApiResponseStatus.Success,
                "Intent Created successfully.",
                clientSecret
            )
        );
    }

    [AllowAnonymous]
    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook(
        [FromServices] IOptions<StripeOptions> stripeOptions,
        CancellationToken ct
    )
    {
        using var reader = new StreamReader(Request.Body);
        var json = await reader.ReadToEndAsync(ct);

        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                stripeOptions.Value.WebhookSecret,
                throwOnApiVersionMismatch: false
            );
        }
        catch (StripeException)
        {
            return BadRequest();
        }

        await paymentService.HandleStripeEventAsync(stripeEvent, ct);
        return Ok();
    }
}
