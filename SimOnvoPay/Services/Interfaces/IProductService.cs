using SimOnvoPay.Models.Common;
using SimOnvoPay.Models.Products;

namespace SimOnvoPay.Services.Interfaces;

public interface IProductService
{
    Task<Product> CreateAsync(CreateProductRequest request, CancellationToken ct = default);
    Task<ListResponse<Product>> ListAsync(ListRequest? request = null, CancellationToken ct = default);
    Task<Product> GetAsync(string productId, CancellationToken ct = default);
    Task<Product> UpdateAsync(string productId, UpdateProductRequest request, CancellationToken ct = default);
    Task<DeletedResponse> DeleteAsync(string productId, CancellationToken ct = default);
}
