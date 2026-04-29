using SimOnvoPay.Http;
using SimOnvoPay.Models.CheckoutSessions;
using SimOnvoPay.Models.Common;
using SimOnvoPay.Services.Interfaces;

namespace SimOnvoPay.Services;

internal class CheckoutSessionService : ICheckoutSessionService
{
    private readonly OnvoPayHttpClient _client;

    public CheckoutSessionService(OnvoPayHttpClient client) => _client = client;

    public Task<CheckoutSession> CreateAsync(CreateCheckoutSessionRequest request, CancellationToken ct = default)
        => _client.PostAsync<CheckoutSession>("v1/checkout-sessions", request, ct);

    public Task<ListResponse<CheckoutSession>> ListAsync(ListRequest? request = null, CancellationToken ct = default)
        => _client.GetAsync<ListResponse<CheckoutSession>>("v1/checkout-sessions", request?.ToQueryParams(), ct);

    public Task<CheckoutSession> GetAsync(string sessionId, CancellationToken ct = default)
        => _client.GetAsync<CheckoutSession>($"v1/checkout-sessions/{sessionId}", null, ct);
}
