using System.ComponentModel.DataAnnotations;

namespace Pickuplay.Teams.Payment;

public sealed class StripeOptions
{
    public const string SectionName = "Stripe";

    [Required] public string SecretKey { get; init; } = null!;

}