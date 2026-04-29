using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimOnvoPay.Configuration;
using SimOnvoPay.Http;
using SimOnvoPay.Services;
using SimOnvoPay.Services.Interfaces;

namespace SimOnvoPay.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOnvoPay(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OnvoPayOptions>(configuration.GetSection(OnvoPayOptions.SectionName));
        return services.AddOnvoPayCore();
    }

    public static IServiceCollection AddOnvoPay(this IServiceCollection services, Action<OnvoPayOptions> configure)
    {
        services.Configure(configure);
        return services.AddOnvoPayCore();
    }

    private static IServiceCollection AddOnvoPayCore(this IServiceCollection services)
    {
        services.AddHttpClient<OnvoPayHttpClient>();

        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IPaymentMethodService, PaymentMethodService>();
        services.AddScoped<IPaymentIntentService, PaymentIntentService>();
        services.AddScoped<IRefundService, RefundService>();
        services.AddScoped<IRecurringChargeService, RecurringChargeService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IPriceService, PriceService>();
        services.AddScoped<ICheckoutSessionService, CheckoutSessionService>();
        services.AddScoped<IMobileTransferService, MobileTransferService>();

        services.AddScoped<OnvoPayClient>();

        return services;
    }
}
