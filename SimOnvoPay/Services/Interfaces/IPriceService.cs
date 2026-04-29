using SimOnvoPay.Models.Common;
using SimOnvoPay.Models.Prices;

namespace SimOnvoPay.Services.Interfaces;

public interface IPriceService
{
    Task<Price> CreateAsync(CreatePriceRequest request, CancellationToken ct = default);
    Task<ListResponse<Price>> ListAsync(ListRequest? request = null, CancellationToken ct = default);
    Task<Price> GetAsync(string priceId, CancellationToken ct = default);
}
