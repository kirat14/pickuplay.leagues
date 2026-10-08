using Microsoft.Extensions.Options;

using Stripe;

namespace Pickuplay.Teams.Payment;

public sealed class PaymentService(IStripeClient stripeClient)
{
    public async Task<string> CreateIntentAsync(long amountInCents, CancellationToken ct)
    {
        var service = new PaymentIntentService(stripeClient);

        var options = new PaymentIntentCreateOptions
        {
            Amount = amountInCents,
            Currency = "usd",
            AllowedPaymentMethodTypes = new List<string> { "card" }
        };

        var intent = await service.CreateAsync(options, cancellationToken: ct);
        return intent.ClientSecret;
    }
}