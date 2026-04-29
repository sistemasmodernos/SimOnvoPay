using SimOnvoPay.Models.CheckoutSessions;
using SimOnvoPay.Models.Common;

namespace SimOnvoPay.Services.Interfaces;

public interface ICheckoutSessionService
{
    Task<CheckoutSession> CreateAsync(CreateCheckoutSessionRequest request, CancellationToken ct = default);
    Task<ListResponse<CheckoutSession>> ListAsync(ListRequest? request = null, CancellationToken ct = default);
    Task<CheckoutSession> GetAsync(string sessionId, CancellationToken ct = default);
}
