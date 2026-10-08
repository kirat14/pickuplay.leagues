using Microsoft.AspNetCore.Mvc;

using Pickuplay.DTOs;
using Pickuplay.Enums;
using Pickuplay.Teams.Payment.DTO;

namespace Pickuplay.Teams.Payment;

[ApiController]
[Route("api/payments")]
public class PaymentsController(PaymentService paymentService) : ControllerBase
{
    [HttpPost("create-intent")]
    public async Task<IActionResult> CreateIntent([FromBody] CreateIntentRequest request, CancellationToken ct)
    {
        var clientSecret = await paymentService.CreateIntentAsync(request.AmountInCents, ct);
        return Ok(new ApiResponse<string>(ApiResponseStatus.Success, "Intent Created successfully.", clientSecret));
    }
}