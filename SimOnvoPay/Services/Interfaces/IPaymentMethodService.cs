using SimOnvoPay.Models.Common;
using SimOnvoPay.Models.PaymentMethods;

namespace SimOnvoPay.Services.Interfaces;

public interface IPaymentMethodService
{
    Task<PaymentMethod> CreateAsync(CreatePaymentMethodRequest request, CancellationToken ct = default);
    Task<ListResponse<PaymentMethod>> ListAsync(ListRequest? request = null, CancellationToken ct = default);
    Task<PaymentMethod> GetAsync(string paymentMethodId, CancellationToken ct = default);
    Task<PaymentMethod> UpdateAsync(string paymentMethodId, UpdatePaymentMethodRequest request, CancellationToken ct = default);
    Task<PaymentMethod> VerifyAsync(string paymentMethodId, VerifyPaymentMethodRequest request, CancellationToken ct = default);
    Task<object> GetVerificationStatusAsync(string paymentMethodId, CancellationToken ct = default);
    Task<DeletedResponse> DisconnectAsync(string paymentMethodId, CancellationToken ct = default);
    Task<object> ValidateBankAccountAsync(string accountNumber, string routingNumber, CancellationToken ct = default);
}
