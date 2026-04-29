using SimOnvoPay.Http;
using SimOnvoPay.Models.Common;
using SimOnvoPay.Models.PaymentMethods;
using SimOnvoPay.Services.Interfaces;

namespace SimOnvoPay.Services;

internal class PaymentMethodService : IPaymentMethodService
{
    private readonly OnvoPayHttpClient _client;

    public PaymentMethodService(OnvoPayHttpClient client) => _client = client;

    public Task<PaymentMethod> CreateAsync(CreatePaymentMethodRequest request, CancellationToken ct = default)
        => _client.PostAsync<PaymentMethod>("v1/payment-methods", request, ct);

    public Task<ListResponse<PaymentMethod>> ListAsync(ListRequest? request = null, CancellationToken ct = default)
        => _client.GetAsync<ListResponse<PaymentMethod>>("v1/payment-methods", request?.ToQueryParams(), ct);

    public Task<PaymentMethod> GetAsync(string paymentMethodId, CancellationToken ct = default)
        => _client.GetAsync<PaymentMethod>($"v1/payment-methods/{paymentMethodId}", null, ct);

    public Task<PaymentMethod> UpdateAsync(string paymentMethodId, UpdatePaymentMethodRequest request, CancellationToken ct = default)
        => _client.PatchAsync<PaymentMethod>($"v1/payment-methods/{paymentMethodId}", request, ct);

    public Task<PaymentMethod> VerifyAsync(string paymentMethodId, VerifyPaymentMethodRequest request, CancellationToken ct = default)
        => _client.PostAsync<PaymentMethod>($"v1/payment-methods/{paymentMethodId}/verify", request, ct);

    public Task<object> GetVerificationStatusAsync(string paymentMethodId, CancellationToken ct = default)
        => _client.GetAsync<object>($"v1/payment-methods/{paymentMethodId}/verification", null, ct);

    public Task<DeletedResponse> DisconnectAsync(string paymentMethodId, CancellationToken ct = default)
        => _client.DeleteAsync<DeletedResponse>($"v1/payment-methods/{paymentMethodId}/disconnect", ct);

    public Task<object> ValidateBankAccountAsync(string accountNumber, string routingNumber, CancellationToken ct = default)
        => _client.PostAsync<object>("v1/payment-methods/validate-bank-account",
            new { accountNumber, routingNumber }, ct);
}
