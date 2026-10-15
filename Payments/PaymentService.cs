using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pickuplay.Teams.Data;
using Pickuplay.Teams.Payments.Enums;
using Pickuplay.Teams.Payments.Models;
using Stripe;

namespace Pickuplay.Teams.Payments;

public sealed class PaymentService(
    IStripeClient stripeClient,
    ILogger<PaymentService> _logger,
    AppDbContext db
)
{
    public async Task<string> CreateIntentAsync(
        long userId,
        string itemType,
        string itemId,
        long amountInCents,
        CancellationToken ct
    )
    {
        var service = new PaymentIntentService(stripeClient);

        var options = new PaymentIntentCreateOptions
        {
            Amount = amountInCents,
            Currency = "usd",
            AllowedPaymentMethodTypes = new List<string> { "card" },
            Metadata = new Dictionary<string, string>
            {
                ["userId"] = userId.ToString(),
                ["itemType"] = itemType,
                ["itemId"] = itemId,
            },
        };

        var intent = await service.CreateAsync(options, cancellationToken: ct);

        db.Payments.Add(
            new PaymentEntity
            {
                UserId = userId,
                ItemType = itemType,
                ItemId = itemId,
                AmountInCents = amountInCents,
                Currency = "usd",
                Status = PaymentStatus.Pending,
                StripePaymentIntentId = intent.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
            }
        );
        await db.SaveChangesAsync(ct);

        return intent.ClientSecret;
    }

    public async Task HandleStripeEventAsync(Event stripeEvent, CancellationToken ct)
    {
        if (stripeEvent.Data.Object is not PaymentIntent intent)
            return;

        _logger.LogInformation(
            "Stripe event {EventId} ({Type}) for intent {IntentId}",
            stripeEvent.Id,
            stripeEvent.Type,
            intent.Id
        );

        var updated = stripeEvent.Type switch
        {
            EventTypes.PaymentIntentSucceeded => await db
                .Payments.Where(p =>
                    p.StripePaymentIntentId == intent.Id && p.Status == PaymentStatus.Pending
                )
                .ExecuteUpdateAsync(
                    setters =>
                        setters
                            .SetProperty(p => p.Status, PaymentStatus.Succeeded)
                            .SetProperty(p => p.PaidAt, stripeEvent.Created),
                    ct
                ),

            EventTypes.PaymentIntentCanceled => await db
                .Payments.Where(p =>
                    p.StripePaymentIntentId == intent.Id && p.Status == PaymentStatus.Pending
                )
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(p => p.Status, PaymentStatus.Canceled),
                    ct
                ),

            EventTypes.PaymentIntentPaymentFailed => await db
                .Payments.Where(p =>
                    p.StripePaymentIntentId == intent.Id && p.Status == PaymentStatus.Pending
                )
                .ExecuteUpdateAsync(
                    s =>
                        s.SetProperty(
                            p => p.FailureReason,
                            intent.LastPaymentError != null ? intent.LastPaymentError.Message : null
                        ),
                    ct
                ),

            _ => -1,
        };

        _logger.LogInformation(
            "Event {EventId} processed, rows updated: {Count}",
            stripeEvent.Id,
            updated
        );
    }
}
