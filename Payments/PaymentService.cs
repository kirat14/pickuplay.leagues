using Microsoft.Extensions.Options;

using Stripe;
using Pickuplay.Teams.Data;
using Pickuplay.Teams.Payments.Enums;
using Pickuplay.Teams.Payments.Models;

namespace Pickuplay.Teams.Payments;

public sealed class PaymentService(IStripeClient stripeClient, ILogger<PaymentService> _logger, AppDbContext db)
{
    public async Task<string> CreateIntentAsync(long userId, string itemType, string itemId, long amountInCents, CancellationToken ct)
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
                ["itemId"] = itemId
            }
        };

        var intent = await service.CreateAsync(options, cancellationToken: ct);

        db.Payments.Add(new PaymentEntity
        {
            UserId = userId,
            ItemType = itemType,
            ItemId = itemId,
            AmountInCents = amountInCents,
            Currency = "usd",
            Status = PaymentStatus.Pending,
            StripePaymentIntentId = intent.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15)
        });
        await db.SaveChangesAsync(ct);

        return intent.ClientSecret;
    }
}