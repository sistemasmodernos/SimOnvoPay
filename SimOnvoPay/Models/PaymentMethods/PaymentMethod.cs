using System.Text.Json;
using System.Text.Json.Serialization;
using SimOnvoPay.Models.Common;

namespace SimOnvoPay.Models.PaymentMethods;

public class PaymentMethod
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("customer")]
    public JsonElement? Customer { get; set; }

    [JsonPropertyName("billingDetails")]
    public BillingDetails? BillingDetails { get; set; }

    [JsonPropertyName("card")]
    public CardDetails? Card { get; set; }

    [JsonPropertyName("bankAccount")]
    public BankAccountDetails? BankAccount { get; set; }

    [JsonPropertyName("mobileNumber")]
    public MobileNumberDetails? MobileNumber { get; set; }

    [JsonPropertyName("liveMode")]
    public bool LiveMode { get; set; }

    [JsonPropertyName("created")]
    public long Created { get; set; }
}

public class BillingDetails
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("address")]
    public Address? Address { get; set; }
}

public class CardDetails
{
    [JsonPropertyName("brand")]
    public string? Brand { get; set; }

    [JsonPropertyName("last4")]
    public string? Last4 { get; set; }

    [JsonPropertyName("expMonth")]
    public int? ExpMonth { get; set; }

    [JsonPropertyName("expYear")]
    public int? ExpYear { get; set; }

    [JsonPropertyName("fingerprint")]
    public string? Fingerprint { get; set; }

    [JsonPropertyName("funding")]
    public string? Funding { get; set; }
}

public class BankAccountDetails
{
    [JsonPropertyName("bankName")]
    public string? BankName { get; set; }

    [JsonPropertyName("last4")]
    public string? Last4 { get; set; }

    [JsonPropertyName("accountType")]
    public string? AccountType { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("routingNumber")]
    public string? RoutingNumber { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}

public class MobileNumberDetails
{
    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("carrier")]
    public string? Carrier { get; set; }
}
