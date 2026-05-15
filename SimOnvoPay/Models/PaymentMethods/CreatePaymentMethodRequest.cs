using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.PaymentMethods;

/// <summary>
/// type: card | bank_account | mobile_number | bank_deposit | credix | zunify | crypto
/// </summary>
public class CreatePaymentMethodRequest
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("customer")]
    public CustomerReference? Customer { get; set; }

    [JsonPropertyName("card")]
    public CreateCardDetails? Card { get; set; }

    [JsonPropertyName("bankAccount")]
    public CreateBankAccountDetails? BankAccount { get; set; }

    [JsonPropertyName("mobileNumber")]
    public CreateMobileNumberDetails? MobileNumber { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }
}

public class CustomerReference
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}

public class CreateCardDetails
{
    [JsonPropertyName("number")]
    public string Number { get; set; } = string.Empty;

    [JsonPropertyName("expMonth")]
    public int ExpMonth { get; set; }

    [JsonPropertyName("expYear")]
    public int ExpYear { get; set; }

    [JsonPropertyName("holderName")]
    public string HolderName { get; set; } = string.Empty;

    [JsonPropertyName("cvv")]
    public string Cvv { get; set; } = string.Empty;
}

public class CreateBankAccountDetails
{
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("routingNumber")]
    public string? RoutingNumber { get; set; }

    [JsonPropertyName("accountType")]
    public string? AccountType { get; set; }
}

public class CreateMobileNumberDetails
{
    /// <summary>Phone number without country code, e.g. "88887777"</summary>
    [JsonPropertyName("number")]
    public string Number { get; set; } = string.Empty;

    /// <summary>0=Cédula, 1=DIMEX, 2=DIDI, 3=Pasaporte, 4=Otro, 5=Jurídica, 9=NITE</summary>
    [JsonPropertyName("identificationType")]
    public int IdentificationType { get; set; }

    [JsonPropertyName("identification")]
    public string Identification { get; set; } = string.Empty;
}
