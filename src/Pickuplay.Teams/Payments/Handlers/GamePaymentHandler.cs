using Microsoft.Extensions.Options;
using Pickuplay.Teams.Payments.Models;
using Pickuplay.Teams.Payments.Options;

namespace Pickuplay.Teams.Payments.Handlers;

public sealed class GamePaymentHandler(
    HttpClient http,
    IOptions<SpringOptions> options,
    ILogger<PaymentService> _logger
) : IPaymentItemHandler
{
    public string ItemType => "Game";

    public async Task OnCancelAsync(string itemId, CancellationToken ct)
    {
        _logger.LogInformation("======= Game Handler for ItemId {} =======", itemId);

        var path = options.Value.GameUserStatusPath.Replace("{itemId}", itemId);

        _logger.LogInformation("======= Internal Path {} =======", itemId);

        var response = await http.PatchAsJsonAsync(path, new { status = "CANCELLED" }, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task OnPaidAsync(string itemId, CancellationToken ct)
    {
        var path = options.Value.GameUserStatusPath.Replace("{itemId}", itemId);

        var response = await http.PatchAsJsonAsync(path, new { status = "JOINED" }, ct);
        response.EnsureSuccessStatusCode();
    }
}
