using SimOnvoPay.Http;
using SimOnvoPay.Models.Common;
using SimOnvoPay.Models.Customers;
using SimOnvoPay.Models.PaymentIntents;
using SimOnvoPay.Models.PaymentMethods;
using SimOnvoPay.Models.RecurringCharges;
using SimOnvoPay.Services.Interfaces;

namespace SimOnvoPay.Services;

internal class CustomerService : ICustomerService
{
    private readonly OnvoPayHttpClient _client;

    public CustomerService(OnvoPayHttpClient client) => _client = client;

    public Task<Customer> CreateAsync(CreateCustomerRequest request, CancellationToken ct = default)
        => _client.PostAsync<Customer>("v1/customers", request, ct);

    public Task<ListResponse<Customer>> ListAsync(CustomerListRequest? request = null, CancellationToken ct = default)
        => _client.GetAsync<ListResponse<Customer>>("v1/customers", request?.ToQueryParams(), ct);

    public Task<Customer> GetAsync(string customerId, CancellationToken ct = default)
        => _client.GetAsync<Customer>($"v1/customers/{customerId}", null, ct);

    public Task<Customer> UpdateAsync(string customerId, UpdateCustomerRequest request, CancellationToken ct = default)
        => _client.PatchAsync<Customer>($"v1/customers/{customerId}", request, ct);

    public Task<DeletedResponse> DeleteAsync(string customerId, CancellationToken ct = default)
        => _client.DeleteAsync<DeletedResponse>($"v1/customers/{customerId}", ct);

    public Task<ListResponse<PaymentMethod>> GetPaymentMethodsAsync(string customerId, CancellationToken ct = default)
        => _client.GetAsync<ListResponse<PaymentMethod>>($"v1/customers/{customerId}/payment-methods", null, ct);

    public Task<ListResponse<PaymentIntent>> GetPaymentIntentsAsync(string customerId, CancellationToken ct = default)
        => _client.GetAsync<ListResponse<PaymentIntent>>($"v1/customers/{customerId}/payment-intents", null, ct);

    public Task<ListResponse<RecurringCharge>> GetRecurringChargesAsync(string customerId, CancellationToken ct = default)
        => _client.GetAsync<ListResponse<RecurringCharge>>($"v1/customers/{customerId}/subscriptions", null, ct);
}
