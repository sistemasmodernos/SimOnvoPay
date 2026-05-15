using Microsoft.EntityFrameworkCore;
using SimOnvoPay.Api.Data;
using SimOnvoPay.Api.Middleware;
using SimOnvoPay.Api.Services;
using SimOnvoPay.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SimOnvoPay API", Version = "v1" });
    c.AddSecurityDefinition("ApiKey", new()
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "X-API-Key",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
});

// OnvoPay SDK
builder.Services.AddOnvoPay(builder.Configuration);

// MySQL con EF Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// Servicios de negocio
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IPaymentSessionService, PaymentSessionService>();
builder.Services.AddHttpClient<ICallbackService, CallbackService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(15);
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = builder.Environment.IsDevelopment()
        ? HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        : null
});

// CORS: permitir el frontend Angular
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? [builder.Configuration["AppSettings:FrontendUrl"] ?? "*"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// Aplicar migraciones automáticamente al arrancar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("FrontendPolicy");

// Permitir embeber el checkout en iframe desde Reservamecr
app.Use(async (ctx, next) =>
{
    ctx.Response.Headers["X-Frame-Options"] = "ALLOWALL";
    ctx.Response.Headers["Content-Security-Policy"] = "frame-ancestors *";
    await next();
});

app.UseMiddleware<ApiKeyMiddleware>();
app.MapControllers();

app.Run();
