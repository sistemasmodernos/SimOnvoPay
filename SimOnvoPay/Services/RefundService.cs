using SimOnvoPay.Http;
using SimOnvoPay.Models.Common;
using SimOnvoPay.Models.Refunds;
using SimOnvoPay.Services.Interfaces;

namespace SimOnvoPay.Services;

internal class RefundService : IRefundService
{
    private readonly OnvoPayHttpClient _client;

    public RefundService(OnvoPayHttpClient client) => _client = client;

    public Task<Refund> CreateAsync(CreateRefundRequest request, CancellationToken ct = default)
        => _client.PostAsync<Refund>("v1/refunds", request, ct);

    public Task<ListResponse<Refund>> ListAsync(ListRequest? request = null, CancellationToken ct = default)
        => _client.GetAsync<ListResponse<Refund>>("v1/refunds", request?.ToQueryParams(), ct);

    public Task<Refund> GetAsync(string refundId, CancellationToken ct = default)
        => _client.GetAsync<Refund>($"v1/refunds/{refundId}", null, ct);
}
