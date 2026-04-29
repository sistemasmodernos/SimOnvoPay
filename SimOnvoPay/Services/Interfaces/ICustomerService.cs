using SimOnvoPay.Models.Common;
using SimOnvoPay.Models.Customers;
using SimOnvoPay.Models.PaymentIntents;
using SimOnvoPay.Models.PaymentMethods;
using SimOnvoPay.Models.RecurringCharges;

namespace SimOnvoPay.Services.Interfaces;

public interface ICustomerService
{
    Task<Customer> CreateAsync(CreateCustomerRequest request, CancellationToken ct = default);
    Task<ListResponse<Customer>> ListAsync(CustomerListRequest? request = null, CancellationToken ct = default);
    Task<Customer> GetAsync(string customerId, CancellationToken ct = default);
    Task<Customer> UpdateAsync(string customerId, UpdateCustomerRequest request, CancellationToken ct = default);
    Task<DeletedResponse> DeleteAsync(string customerId, CancellationToken ct = default);
    Task<ListResponse<PaymentMethod>> GetPaymentMethodsAsync(string customerId, CancellationToken ct = default);
    Task<ListResponse<PaymentIntent>> GetPaymentIntentsAsync(string customerId, CancellationToken ct = default);
    Task<ListResponse<RecurringCharge>> GetRecurringChargesAsync(string customerId, CancellationToken ct = default);
}
