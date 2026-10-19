using Pickuplay.Teams.Payments.Models;

namespace Pickuplay.Teams.Payments.Handlers;

public interface IPaymentItemHandler
{
    string ItemType { get; }
    Task OnPaidAsync(string itemId, CancellationToken ct);
}
