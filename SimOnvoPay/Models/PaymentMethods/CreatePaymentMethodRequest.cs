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
    public string? Customer { get; set; }

    [JsonPropertyName("billingDetails")]
    public BillingDetails? BillingDetails { get; set; }

    [JsonPropertyName("card")]
    public CreateCardDetails? Card { get; set; }

    [JsonPropertyName("bankAccount")]
    public CreateBankAccountDetails? BankAccount { get; set; }

    [JsonPropertyName("mobileNumber")]
    public CreateMobileNumberDetails? MobileNumber { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }
}

public class CreateCardDetails
{
    [JsonPropertyName("number")]
    public string Number { get; set; } = string.Empty;

    [JsonPropertyName("expMonth")]
    public int ExpMonth { get; set; }

    [JsonPropertyName("expYear")]
    public int ExpYear { get; set; }

    [JsonPropertyName("cvc")]
    public string Cvc { get; set; } = string.Empty;
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
    [JsonPropertyName("phone")]
    public string Phone { get; set; } = string.Empty;
}
