using SimOnvoPay.Http;
using SimOnvoPay.Models.Common;
using SimOnvoPay.Models.Products;
using SimOnvoPay.Services.Interfaces;

namespace SimOnvoPay.Services;

internal class ProductService : IProductService
{
    private readonly OnvoPayHttpClient _client;

    public ProductService(OnvoPayHttpClient client) => _client = client;

    public Task<Product> CreateAsync(CreateProductRequest request, CancellationToken ct = default)
        => _client.PostAsync<Product>("v1/products", request, ct);

    public Task<ListResponse<Product>> ListAsync(ListRequest? request = null, CancellationToken ct = default)
        => _client.GetAsync<ListResponse<Product>>("v1/products", request?.ToQueryParams(), ct);

    public Task<Product> GetAsync(string productId, CancellationToken ct = default)
        => _client.GetAsync<Product>($"v1/products/{productId}", null, ct);

    public Task<Product> UpdateAsync(string productId, UpdateProductRequest request, CancellationToken ct = default)
        => _client.PatchAsync<Product>($"v1/products/{productId}", request, ct);

    public Task<DeletedResponse> DeleteAsync(string productId, CancellationToken ct = default)
        => _client.DeleteAsync<DeletedResponse>($"v1/products/{productId}", ct);
}
