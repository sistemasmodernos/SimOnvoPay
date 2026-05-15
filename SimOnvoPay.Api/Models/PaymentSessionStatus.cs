namespace SimOnvoPay.Api.Models;

public enum PaymentSessionStatus
{
    Pending = 0,
    Processing = 1,
    Succeeded = 2,
    Failed = 3,
    Cancelled = 4,
    Expired = 5
}
