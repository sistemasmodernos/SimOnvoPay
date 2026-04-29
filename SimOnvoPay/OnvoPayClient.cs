using SimOnvoPay.Services.Interfaces;

namespace SimOnvoPay;

public class OnvoPayClient
{
    public ICustomerService Customers { get; }
    public IPaymentMethodService PaymentMethods { get; }
    public IPaymentIntentService PaymentIntents { get; }
    public IRefundService Refunds { get; }
    public IRecurringChargeService RecurringCharges { get; }
    public IProductService Products { get; }
    public IPriceService Prices { get; }
    public ICheckoutSessionService CheckoutSessions { get; }
    public IMobileTransferService MobileTransfers { get; }

    public OnvoPayClient(
        ICustomerService customers,
        IPaymentMethodService paymentMethods,
        IPaymentIntentService paymentIntents,
        IRefundService refunds,
        IRecurringChargeService recurringCharges,
        IProductService products,
        IPriceService prices,
        ICheckoutSessionService checkoutSessions,
        IMobileTransferService mobileTransfers)
    {
        Customers = customers;
        PaymentMethods = paymentMethods;
        PaymentIntents = paymentIntents;
        Refunds = refunds;
        RecurringCharges = recurringCharges;
        Products = products;
        Prices = prices;
        CheckoutSessions = checkoutSessions;
        MobileTransfers = mobileTransfers;
    }
}
