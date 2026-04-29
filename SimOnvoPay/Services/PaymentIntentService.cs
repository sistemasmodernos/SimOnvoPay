using SimOnvoPay.Http;
using SimOnvoPay.Models.Common;
using SimOnvoPay.Models.PaymentIntents;
using SimOnvoPay.Services.Interfaces;

namespace SimOnvoPay.Services;

internal class PaymentIntentService : IPaymentIntentService
{
    private readonly OnvoPayHttpClient _client;

    public PaymentIntentService(OnvoPayHttpClient client) => _client = client;

    public Task<PaymentIntent> CreateAsync(CreatePaymentIntentRequest request, CancellationToken ct = default)
        => _client.PostAsync<PaymentIntent>("v1/payment-intents", request, ct);

    public Task<ListResponse<PaymentIntent>> ListAsync(PaymentIntentListRequest? request = null, CancellationToken ct = default)
        => _client.GetAsync<ListResponse<PaymentIntent>>("v1/payment-intents", request?.ToQueryParams(), ct);

    public Task<PaymentIntent> GetAsync(string paymentIntentId, CancellationToken ct = default)
        => _client.GetAsync<PaymentIntent>($"v1/payment-intents/{paymentIntentId}", null, ct);

    public Task<PaymentIntent> UpdateAsync(string paymentIntentId, UpdatePaymentIntentRequest request, CancellationToken ct = default)
        => _client.PatchAsync<PaymentIntent>($"v1/payment-intents/{paymentIntentId}", request, ct);

    public Task<PaymentIntent> ConfirmAsync(string paymentIntentId, ConfirmPaymentIntentRequest? request = null, CancellationToken ct = default)
        => _client.PostAsync<PaymentIntent>($"v1/payment-intents/{paymentIntentId}/confirm", request, ct);

    public Task<PaymentIntent> CaptureAsync(string paymentIntentId, CapturePaymentIntentRequest? request = null, CancellationToken ct = default)
        => _client.PostAsync<PaymentIntent>($"v1/payment-intents/{paymentIntentId}/capture", request, ct);

    public Task<PaymentIntent> CancelAsync(string paymentIntentId, CancellationToken ct = default)
        => _client.PostAsync<PaymentIntent>($"v1/payment-intents/{paymentIntentId}/cancel", null, ct);
}
