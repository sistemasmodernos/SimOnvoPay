using SimOnvoPay.Models.Common;
using SimOnvoPay.Models.Refunds;

namespace SimOnvoPay.Services.Interfaces;

public interface IRefundService
{
    Task<Refund> CreateAsync(CreateRefundRequest request, CancellationToken ct = default);
    Task<ListResponse<Refund>> ListAsync(ListRequest? request = null, CancellationToken ct = default);
    Task<Refund> GetAsync(string refundId, CancellationToken ct = default);
}
