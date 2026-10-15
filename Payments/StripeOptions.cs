using System.ComponentModel.DataAnnotations;

namespace Pickuplay.Teams.Payments;

public sealed class StripeOptions
{
    public const string SectionName = "Stripe";

    [Required] public string SecretKey { get; init; } = null!;
    [Required] public string WebhookSecret { get; init; } = default!;

}