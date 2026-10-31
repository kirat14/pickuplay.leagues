using Pickuplay.Teams.Payments.Enums;

namespace Pickuplay.Teams.Payments.Models;

public class PaymentEntity
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string ItemType { get; set; } = default!;
    public string ItemId { get; set; } = default!;
    public long AmountInCents { get; set; }
    public string Currency { get; set; } = "usd";
    public PaymentStatus Status { get; set; }
    public string StripePaymentIntentId { get; set; } = default!;
    public string? FailureReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? PaidAt { get; set; }
}