using SimOnvoPay.Http;
using SimOnvoPay.Models.Common;
using SimOnvoPay.Models.Prices;
using SimOnvoPay.Services.Interfaces;

namespace SimOnvoPay.Services;

internal class PriceService : IPriceService
{
    private readonly OnvoPayHttpClient _client;

    public PriceService(OnvoPayHttpClient client) => _client = client;

    public Task<Price> CreateAsync(CreatePriceRequest request, CancellationToken ct = default)
        => _client.PostAsync<Price>("v1/prices", request, ct);

    public Task<ListResponse<Price>> ListAsync(ListRequest? request = null, CancellationToken ct = default)
        => _client.GetAsync<ListResponse<Price>>("v1/prices", request?.ToQueryParams(), ct);

    public Task<Price> GetAsync(string priceId, CancellationToken ct = default)
        => _client.GetAsync<Price>($"v1/prices/{priceId}", null, ct);
}
