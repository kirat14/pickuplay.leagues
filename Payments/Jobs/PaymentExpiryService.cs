using Microsoft.EntityFrameworkCore;
using Pickuplay.Teams.Data;
using Pickuplay.Teams.Payments.Enums;
using Pickuplay.Teams.Payments.Handlers;
using Pickuplay.Teams.Payments.Models;
using Stripe;

namespace Pickuplay.Teams.Payments.Jobs;

public sealed class PaymentExpiryService(
    IServiceScopeFactory scopeFactory,
    ILogger<PaymentExpiryService> logger,
    IEnumerable<IPaymentItemHandler> _handlers
) : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("PaymentExpiryService started");

        using var timer = new PeriodicTimer(Interval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var stripeClient = scope.ServiceProvider.GetRequiredService<IStripeClient>();

                var now = DateTime.UtcNow;
                var expired = await db
                    .Payments.Where(p => p.Status == PaymentStatus.Pending && p.ExpiresAt <= now)
                    .OrderBy(p => p.ExpiresAt)
                    .Take(50)
                    .ToListAsync(stoppingToken);

                if (expired.Count > 0)
                {
                    logger.LogInformation("Found {Count} expired payments", expired.Count);
                    await CancelExpiredIntentsAsync(db, stripeClient, expired, stoppingToken);
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogError(ex, "Payment expiry run failed");
            }
        }
    }

    private async Task CancelExpiredIntentsAsync(
        AppDbContext db,
        IStripeClient stripeClient,
        List<PaymentEntity> expired,
        CancellationToken ct
    )
    {
        var intents = new PaymentIntentService(stripeClient);

        foreach (var payment in expired)
        {
            try
            {
                await intents.CancelAsync(payment.StripePaymentIntentId, cancellationToken: ct);

                payment.Status = PaymentStatus.Canceled;
                await db.SaveChangesAsync(ct);
                await GetHandler(payment.ItemType).OnCancelAsync(payment.ItemId, ct);
            }
            catch (StripeException)
            {
                var intent = await intents.GetAsync(
                    payment.StripePaymentIntentId,
                    cancellationToken: ct
                );

                if (intent.Status == "succeeded")
                {
                    logger.LogWarning("Payment {Id} succeeded at expiry, skipping", payment.Id);
                }
                else if (intent.Status == "canceled")
                {
                    payment.Status = PaymentStatus.Canceled;

                    await db.SaveChangesAsync(ct);
                }
                else
                {
                    logger.LogError(
                        "Could not cancel payment {Id}, intent status {Status}",
                        payment.Id,
                        intent.Status
                    );
                }
            }
        }
    }

    private IPaymentItemHandler GetHandler(string itemType) =>
        _handlers.FirstOrDefault(h =>
            string.Equals(h.ItemType, itemType, StringComparison.OrdinalIgnoreCase)
        ) ?? throw new InvalidOperationException($"No handler for item type {itemType}");
}
