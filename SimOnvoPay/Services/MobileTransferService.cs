using SimOnvoPay.Http;
using SimOnvoPay.Models.Common;
using SimOnvoPay.Models.MobileTransfers;
using SimOnvoPay.Services.Interfaces;

namespace SimOnvoPay.Services;

internal class MobileTransferService : IMobileTransferService
{
    private readonly OnvoPayHttpClient _client;

    public MobileTransferService(OnvoPayHttpClient client) => _client = client;

    public Task<ListResponse<MobileTransfer>> ListAsync(ListRequest? request = null, CancellationToken ct = default)
        => _client.GetAsync<ListResponse<MobileTransfer>>("v1/mobile-transfers", request?.ToQueryParams(), ct);

    public Task<MobileTransfer> GetAsync(string transferId, CancellationToken ct = default)
        => _client.GetAsync<MobileTransfer>($"v1/mobile-transfers/{transferId}", null, ct);
}
