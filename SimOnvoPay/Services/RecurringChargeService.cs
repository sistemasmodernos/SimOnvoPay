using SimOnvoPay.Http;
using SimOnvoPay.Models.Common;
using SimOnvoPay.Models.RecurringCharges;
using SimOnvoPay.Services.Interfaces;

namespace SimOnvoPay.Services;

internal class RecurringChargeService : IRecurringChargeService
{
    private readonly OnvoPayHttpClient _client;

    public RecurringChargeService(OnvoPayHttpClient client) => _client = client;

    public Task<RecurringCharge> CreateAsync(CreateRecurringChargeRequest request, CancellationToken ct = default)
        => _client.PostAsync<RecurringCharge>("v1/subscriptions", request, ct);

    public Task<ListResponse<RecurringCharge>> ListAsync(ListRequest? request = null, CancellationToken ct = default)
        => _client.GetAsync<ListResponse<RecurringCharge>>("v1/subscriptions", request?.ToQueryParams(), ct);

    public Task<RecurringCharge> GetAsync(string subscriptionId, CancellationToken ct = default)
        => _client.GetAsync<RecurringCharge>($"v1/subscriptions/{subscriptionId}", null, ct);

    public Task<RecurringCharge> CancelAsync(string subscriptionId, bool cancelAtPeriodEnd = false, CancellationToken ct = default)
        => _client.PatchAsync<RecurringCharge>($"v1/subscriptions/{subscriptionId}", new { cancelAtPeriodEnd }, ct);
}
