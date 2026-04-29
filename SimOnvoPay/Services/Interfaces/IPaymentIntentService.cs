using SimOnvoPay.Models.Common;
using SimOnvoPay.Models.PaymentIntents;

namespace SimOnvoPay.Services.Interfaces;

public interface IPaymentIntentService
{
    Task<PaymentIntent> CreateAsync(CreatePaymentIntentRequest request, CancellationToken ct = default);
    Task<ListResponse<PaymentIntent>> ListAsync(PaymentIntentListRequest? request = null, CancellationToken ct = default);
    Task<PaymentIntent> GetAsync(string paymentIntentId, CancellationToken ct = default);
    Task<PaymentIntent> UpdateAsync(string paymentIntentId, UpdatePaymentIntentRequest request, CancellationToken ct = default);
    Task<PaymentIntent> ConfirmAsync(string paymentIntentId, ConfirmPaymentIntentRequest? request = null, CancellationToken ct = default);
    Task<PaymentIntent> CaptureAsync(string paymentIntentId, CapturePaymentIntentRequest? request = null, CancellationToken ct = default);
    Task<PaymentIntent> CancelAsync(string paymentIntentId, CancellationToken ct = default);
}
