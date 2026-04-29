namespace SimOnvoPay.Configuration;

public class OnvoPayOptions
{
    public const string SectionName = "OnvoPay";

    public string SecretKey { get; set; } = string.Empty;
    public string PublishableKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.onvopay.com";
    public int TimeoutSeconds { get; set; } = 30;
    public bool IsLiveMode => SecretKey.StartsWith("onvo_live_");
}
