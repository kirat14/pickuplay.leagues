using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pickuplay.DTOs;
using Pickuplay.Enums;
using Pickuplay.Teams.Payments.DTO;

namespace Pickuplay.Teams.Payments;

[ApiController]
[Route("api/payments")]
public class PaymentsController(PaymentService paymentService) : ControllerBase
{
    [HttpPost("create-intent")]
    [Authorize]
    public async Task<IActionResult> CreateIntent([FromBody] CreateIntentRequest request, CancellationToken ct)
    {
        var userId = User.FindFirst("id")?.Value;

        if (!int.TryParse(userId, out var playerId))
        {
            throw new UnauthorizedAccessException("User ID could not be found in the token.");
        }

        var clientSecret = await paymentService.CreateIntentAsync(
            playerId, request.ItemType, request.ItemId, request.AmountInCents, ct); return Ok(new ApiResponse<string>(ApiResponseStatus.Success, "Intent Created successfully.", clientSecret));
    }
}