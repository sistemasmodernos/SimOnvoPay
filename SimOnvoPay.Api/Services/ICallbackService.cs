using SimOnvoPay.Api.Models;

namespace SimOnvoPay.Api.Services;

public interface ICallbackService
{
    Task NotifyAsync(PaymentSession session);
}
