using SimOnvoPay.Models.Common;
using SimOnvoPay.Models.RecurringCharges;

namespace SimOnvoPay.Services.Interfaces;

public interface IRecurringChargeService
{
    Task<RecurringCharge> CreateAsync(CreateRecurringChargeRequest request, CancellationToken ct = default);
    Task<ListResponse<RecurringCharge>> ListAsync(ListRequest? request = null, CancellationToken ct = default);
    Task<RecurringCharge> GetAsync(string subscriptionId, CancellationToken ct = default);
    Task<RecurringCharge> CancelAsync(string subscriptionId, bool cancelAtPeriodEnd = false, CancellationToken ct = default);
}
