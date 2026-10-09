namespace Pickuplay.Teams.Payments.DTO;

public record CreateIntentRequest(string ItemType, string ItemId, long AmountInCents);