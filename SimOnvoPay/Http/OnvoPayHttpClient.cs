using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using Microsoft.Extensions.Options;
using SimOnvoPay.Configuration;
using SimOnvoPay.Exceptions;

namespace SimOnvoPay.Http;

internal class OnvoPayHttpClient
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true
    };

    public OnvoPayHttpClient(HttpClient httpClient, IOptions<OnvoPayOptions> options)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(options.Value.BaseUrl.TrimEnd('/') + "/");
        _httpClient.Timeout = TimeSpan.FromSeconds(options.Value.TimeoutSeconds);
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", options.Value.SecretKey);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<T> GetAsync<T>(string path, Dictionary<string, string>? queryParams = null, CancellationToken ct = default)
    {
        var url = BuildUrl(path, queryParams);
        var response = await _httpClient.GetAsync(url, ct);
        return await HandleResponseAsync<T>(response, ct);
    }

    public async Task<T> PostAsync<T>(string path, object? body = null, CancellationToken ct = default)
    {
        var content = body is null ? null : JsonContent.Create(body, options: JsonOptions);
        var response = await _httpClient.PostAsync(path, content, ct);
        return await HandleResponseAsync<T>(response, ct);
    }

    public async Task<T> PatchAsync<T>(string path, object body, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(body, JsonOptions);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Patch, path) { Content = content };
        var response = await _httpClient.SendAsync(request, ct);
        return await HandleResponseAsync<T>(response, ct);
    }

    public async Task<T> DeleteAsync<T>(string path, CancellationToken ct = default)
    {
        var response = await _httpClient.DeleteAsync(path, ct);
        return await HandleResponseAsync<T>(response, ct);
    }

    private static string BuildUrl(string path, Dictionary<string, string>? queryParams)
    {
        if (queryParams is null || queryParams.Count == 0) return path;
        var query = string.Join("&", queryParams.Select(kv =>
            $"{HttpUtility.UrlEncode(kv.Key)}={HttpUtility.UrlEncode(kv.Value)}"));
        return $"{path}?{query}";
    }

    private static async Task<T> HandleResponseAsync<T>(HttpResponseMessage response, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<T>(JsonOptions, ct);
            return result ?? throw new OnvoPayException((int)response.StatusCode, "La respuesta de la API fue vacía.");
        }

        var errorBody = await response.Content.ReadAsStringAsync(ct);
        string? apiCode = null;
        string? apiMessage = null;

        try
        {
            var error = JsonSerializer.Deserialize<ApiError>(errorBody, JsonOptions);
            apiCode = error?.Code;
            apiMessage = error?.Message;
        }
        catch { }

        throw new OnvoPayException(
            (int)response.StatusCode,
            $"Error OnvoPay [{(int)response.StatusCode}]: {apiMessage ?? errorBody}",
            apiCode,
            apiMessage
        );
    }

    private record ApiError(
        [property: JsonPropertyName("code")] string? Code,
        [property: JsonPropertyName("message")] string? Message
    );
}
