namespace SimOnvoPay.Api.Middleware;

public class ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private const string ApiKeyHeader = "X-API-Key";

    public async Task InvokeAsync(HttpContext context)
    {
        // Solo proteger las rutas de Sessions (para sistemas externos)
        if (context.Request.Path.StartsWithSegments("/api/v1/sessions"))
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var providedKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "API Key requerida." });
                return;
            }

            var validKey = configuration["AppSettings:ApiKey"];
            if (string.IsNullOrEmpty(validKey) || providedKey != validKey)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new { error = "API Key inválida." });
                return;
            }
        }

        await next(context);
    }
}
