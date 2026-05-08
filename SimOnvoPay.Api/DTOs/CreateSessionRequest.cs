using System.ComponentModel.DataAnnotations;

namespace SimOnvoPay.Api.DTOs;

public class CreateSessionRequest
{
    [Required]
    [Range(1, long.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
    public long Amount { get; set; }

    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string Currency { get; set; } = "CRC";

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Url]
    public string CallbackUrl { get; set; } = string.Empty;

    [Required]
    [Url]
    public string SuccessUrl { get; set; } = string.Empty;

    [Required]
    [Url]
    public string CancelUrl { get; set; } = string.Empty;

    [StringLength(200)]
    public string? ExternalReference { get; set; }

    public Dictionary<string, string>? Metadata { get; set; }
}
