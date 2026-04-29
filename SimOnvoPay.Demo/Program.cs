using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimOnvoPay;
using SimOnvoPay.Exceptions;
using SimOnvoPay.Extensions;
using SimOnvoPay.Models.Customers;
using SimOnvoPay.Models.PaymentIntents;
using SimOnvoPay.Models.PaymentMethods;
using SimOnvoPay.Models.Products;
using SimOnvoPay.Models.Prices;
using SimOnvoPay.Models.Refunds;
using SimOnvoPay.Models.CheckoutSessions;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();
services.AddOnvoPay(config);
var provider = services.BuildServiceProvider();

var onvo = provider.GetRequiredService<OnvoPayClient>();

Console.WriteLine("=== SimOnvoPay Demo ===\n");

await DemoClientes(onvo);
await DemoMetodosDePago(onvo);
await DemoIntencionDePago(onvo);
await DemoReembolso(onvo);
await DemoProductosYPrecios(onvo);
await DemoCheckoutSession(onvo);
await DemoTransferenciasMoviles(onvo);

Console.WriteLine("\n=== Demo completado ===");

// ── Clientes ─────────────────────────────────────────────────────────────────
static async Task DemoClientes(OnvoPayClient onvo)
{
    Console.WriteLine("--- CLIENTES ---");
    try
    {
        var cliente = await onvo.Customers.CreateAsync(new CreateCustomerRequest
        {
            Name = "Juan Pérez",
            Email = "juan.perez@ejemplo.cr",
            Phone = "+50688887777",
            Metadata = new() { ["sistema"] = "Reserva2", ["origen"] = "demo" }
        });
        Console.WriteLine($"  Creado: {cliente.Id} - {cliente.Name}");

        var actualizado = await onvo.Customers.UpdateAsync(cliente.Id, new UpdateCustomerRequest
        {
            Name = "Juan Pérez Mora"
        });
        Console.WriteLine($"  Actualizado: {actualizado.Name}");

        var lista = await onvo.Customers.ListAsync(new CustomerListRequest { Limit = 5 });
        Console.WriteLine($"  Total clientes: {lista.Meta.Total}");

        var eliminado = await onvo.Customers.DeleteAsync(cliente.Id);
        Console.WriteLine($"  Eliminado: {eliminado.Deleted}");
    }
    catch (OnvoPayException ex)
    {
        Console.WriteLine($"  [Error {ex.StatusCode}] {ex.Message}");
    }
}

// ── Métodos de Pago ───────────────────────────────────────────────────────────
static async Task DemoMetodosDePago(OnvoPayClient onvo)
{
    Console.WriteLine("\n--- MÉTODOS DE PAGO ---");
    try
    {
        var tarjeta = await onvo.PaymentMethods.CreateAsync(new CreatePaymentMethodRequest
        {
            Type = "card",
            Card = new CreateCardDetails
            {
                Number = "4242424242424242",
                ExpMonth = 12,
                ExpYear = 2027,
                Cvc = "123"
            },
            BillingDetails = new BillingDetails { Name = "Juan Pérez" }
        });
        Console.WriteLine($"  Tarjeta creada: {tarjeta.Id} - {tarjeta.Card?.Brand} *{tarjeta.Card?.Last4}");

        var sinpe = await onvo.PaymentMethods.CreateAsync(new CreatePaymentMethodRequest
        {
            Type = "mobile_number",
            MobileNumber = new CreateMobileNumberDetails { Phone = "88887777" }
        });
        Console.WriteLine($"  SINPE Móvil: {sinpe.Id} - {sinpe.MobileNumber?.Phone}");

        var lista = await onvo.PaymentMethods.ListAsync();
        Console.WriteLine($"  Total métodos: {lista.Meta.Total}");
    }
    catch (OnvoPayException ex)
    {
        Console.WriteLine($"  [Error {ex.StatusCode}] {ex.Message}");
    }
}

// ── Intención de Pago ─────────────────────────────────────────────────────────
static async Task DemoIntencionDePago(OnvoPayClient onvo)
{
    Console.WriteLine("\n--- INTENCIONES DE PAGO ---");
    try
    {
        var intencion = await onvo.PaymentIntents.CreateAsync(new CreatePaymentIntentRequest
        {
            Amount = 10000,
            Currency = "CRC",
            Description = "Reserva sala de reuniones #42",
            CaptureMethod = "automatic",
            Metadata = new() { ["reserva_id"] = "42" }
        });
        Console.WriteLine($"  Creada: {intencion.Id} - {intencion.Status} - {intencion.Amount} {intencion.Currency}");

        var cancelada = await onvo.PaymentIntents.CancelAsync(intencion.Id);
        Console.WriteLine($"  Cancelada: {cancelada.Status}");

        var lista = await onvo.PaymentIntents.ListAsync(new PaymentIntentListRequest { Limit = 5 });
        Console.WriteLine($"  Total intenciones: {lista.Meta.Total}");
    }
    catch (OnvoPayException ex)
    {
        Console.WriteLine($"  [Error {ex.StatusCode}] {ex.Message}");
    }
}

// ── Reembolsos ────────────────────────────────────────────────────────────────
static async Task DemoReembolso(OnvoPayClient onvo)
{
    Console.WriteLine("\n--- REEMBOLSOS ---");
    try
    {
        // Requiere un payment_intent con status succeeded para reembolsar.
        // var reembolso = await onvo.Refunds.CreateAsync(new CreateRefundRequest
        // {
        //     PaymentIntent = "pi_xxxx",
        //     Amount = 5000,
        //     Reason = "requested_by_customer"
        // });

        var lista = await onvo.Refunds.ListAsync();
        Console.WriteLine($"  Total reembolsos: {lista.Meta.Total}");
    }
    catch (OnvoPayException ex)
    {
        Console.WriteLine($"  [Error {ex.StatusCode}] {ex.Message}");
    }
}

// ── Productos y Precios ───────────────────────────────────────────────────────
static async Task DemoProductosYPrecios(OnvoPayClient onvo)
{
    Console.WriteLine("\n--- PRODUCTOS Y PRECIOS ---");
    try
    {
        var producto = await onvo.Products.CreateAsync(new CreateProductRequest
        {
            Name = "Sala de Reuniones Premium",
            Description = "Acceso por hora a sala equipada",
            Active = true
        });
        Console.WriteLine($"  Producto: {producto.Id} - {producto.Name}");

        var precio = await onvo.Prices.CreateAsync(new CreatePriceRequest
        {
            Product = producto.Id,
            Currency = "CRC",
            UnitAmount = 15000,
            Type = "one_time"
        });
        Console.WriteLine($"  Precio único: {precio.Id} - {precio.UnitAmount} {precio.Currency}");

        var precioMensual = await onvo.Prices.CreateAsync(new CreatePriceRequest
        {
            Product = producto.Id,
            Currency = "CRC",
            UnitAmount = 500000,
            Type = "recurring",
            Recurring = new RecurringInterval { Interval = "month", IntervalCount = 1 }
        });
        Console.WriteLine($"  Precio mensual: {precioMensual.Id} - cada {precioMensual.Recurring?.IntervalCount} {precioMensual.Recurring?.Interval}");
    }
    catch (OnvoPayException ex)
    {
        Console.WriteLine($"  [Error {ex.StatusCode}] {ex.Message}");
    }
}

// ── Checkout Session ──────────────────────────────────────────────────────────
static async Task DemoCheckoutSession(OnvoPayClient onvo)
{
    Console.WriteLine("\n--- CHECKOUT SESSION ---");
    try
    {
        var session = await onvo.CheckoutSessions.CreateAsync(new CreateCheckoutSessionRequest
        {
            SuccessUrl = "https://mireserva.cr/pago/exitoso?session={CHECKOUT_SESSION_ID}",
            CancelUrl = "https://mireserva.cr/pago/cancelado",
            Mode = "payment",
            Metadata = new() { ["reserva_id"] = "42" }
        });
        Console.WriteLine($"  Session: {session.Id} - {session.Status}");
        Console.WriteLine($"  URL de pago: {session.Url}");
    }
    catch (OnvoPayException ex)
    {
        Console.WriteLine($"  [Error {ex.StatusCode}] {ex.Message}");
    }
}

// ── Transferencias Móviles (SINPE Móvil) ─────────────────────────────────────
static async Task DemoTransferenciasMoviles(OnvoPayClient onvo)
{
    Console.WriteLine("\n--- TRANSFERENCIAS MÓVILES (SINPE) ---");
    try
    {
        var lista = await onvo.MobileTransfers.ListAsync();
        Console.WriteLine($"  Total transferencias: {lista.Meta.Total}");
        foreach (var t in lista.Data.Take(3))
            Console.WriteLine($"  {t.Id} | {t.Status} | {t.Amount} {t.Currency} | Tel: {t.Phone}");
    }
    catch (OnvoPayException ex)
    {
        Console.WriteLine($"  [Error {ex.StatusCode}] {ex.Message}");
    }
}
