using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pickuplay.Teams.Data;
using Pickuplay.Teams.Payments.Enums;
using Pickuplay.Teams.Payments.Handlers;
using Pickuplay.Teams.Payments.Models;
using Stripe;

namespace Pickuplay.Teams.Payments;

public sealed class PaymentService(
    IStripeClient stripeClient,
    ILogger<PaymentService> _logger,
    IEnumerable<IPaymentItemHandler> _handlers,
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

        var pending = db.Payments.Where(p =>
            p.StripePaymentIntentId == intent.Id && p.Status == PaymentStatus.Pending
        );

        switch (stripeEvent.Type)
        {
            case EventTypes.PaymentIntentSucceeded:
                await pending.ExecuteUpdateAsync(
                    setters =>
                        setters
                            .SetProperty(p => p.Status, PaymentStatus.Succeeded)
                            .SetProperty(p => p.PaidAt, stripeEvent.Created),
                    ct
                );

                var payment = await db
                    .Payments.AsNoTracking()
                    .FirstOrDefaultAsync(
                        p =>
                            p.StripePaymentIntentId == intent.Id
                            && p.Status == PaymentStatus.Succeeded,
                        ct
                    );

                if (payment is not null)
                    await GetHandler(payment.ItemType).OnPaidAsync(payment.ItemId, ct);
                break;

            case EventTypes.PaymentIntentCanceled:
                await pending.ExecuteUpdateAsync(
                    setters => setters.SetProperty(p => p.Status, PaymentStatus.Canceled),
                    ct
                );
                break;

            case EventTypes.PaymentIntentPaymentFailed:
                await pending.ExecuteUpdateAsync(
                    setters =>
                        setters.SetProperty(
                            p => p.FailureReason,
                            intent.LastPaymentError != null ? intent.LastPaymentError.Message : null
                        ),
                    ct
                );
                break;
        }

        _logger.LogInformation("Event {EventId} processed", stripeEvent.Id);
    }

    private IPaymentItemHandler GetHandler(string itemType) =>
        _handlers.FirstOrDefault(h =>
            string.Equals(h.ItemType, itemType, StringComparison.OrdinalIgnoreCase)
        ) ?? throw new InvalidOperationException($"No handler for item type {itemType}");
}
