namespace SimOnvoPay.Api.DTOs;

public class CreateSessionResponse
{
    public string Token { get; set; } = string.Empty;
    public string CheckoutUrl { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
