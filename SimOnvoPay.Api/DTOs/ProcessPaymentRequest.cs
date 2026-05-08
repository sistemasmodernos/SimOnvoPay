using System.ComponentModel.DataAnnotations;

namespace SimOnvoPay.Api.DTOs;

public class ProcessPaymentRequest
{
    [Required]
    [StringLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required]
    public string CustomerPhone { get; set; } = string.Empty;

    [Required]
    public string PaymentMethod { get; set; } = string.Empty; // card | sinpe | sinpe_movil

    public CardPaymentDetails? Card { get; set; }
    public SinpePaymentDetails? Sinpe { get; set; }
    public SinpeMovilPaymentDetails? SinpeMovil { get; set; }
}

public class CardPaymentDetails
{
    [Required]
    public string Number { get; set; } = string.Empty;

    [Required]
    [Range(1, 12)]
    public int ExpMonth { get; set; }

    [Required]
    [Range(2024, 2099)]
    public int ExpYear { get; set; }

    [Required]
    public string Cvc { get; set; } = string.Empty;
}

public class SinpePaymentDetails
{
    [Required]
    public string AccountNumber { get; set; } = string.Empty;

    public string AccountType { get; set; } = "checking";
}

public class SinpeMovilPaymentDetails
{
    [Required]
    public string Phone { get; set; } = string.Empty;
}
