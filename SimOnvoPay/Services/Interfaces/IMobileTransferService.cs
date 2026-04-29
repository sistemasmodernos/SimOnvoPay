using SimOnvoPay.Models.Common;
using SimOnvoPay.Models.MobileTransfers;

namespace SimOnvoPay.Services.Interfaces;

public interface IMobileTransferService
{
    Task<ListResponse<MobileTransfer>> ListAsync(ListRequest? request = null, CancellationToken ct = default);
    Task<MobileTransfer> GetAsync(string transferId, CancellationToken ct = default);
}
