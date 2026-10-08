using Microsoft.AspNetCore.Mvc;

namespace Pickuplay.Teams.Payment;

[ApiController]
[Route("api/payments")]
public class PaymentsController(PaymentService paymentService) : ControllerBase
{
    [HttpPost("create-intent")]
    public async Task<IActionResult> CreateIntent(CreateIntentRequest request, CancellationToken ct)
    {
        var clientSecret = await paymentService.CreateIntentAsync(request.AmountInCents, ct);
        return Ok(new { clientSecret });
    }
}

public record CreateIntentRequest(long AmountInCents);