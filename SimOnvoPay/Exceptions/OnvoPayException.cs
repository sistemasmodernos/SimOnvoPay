namespace SimOnvoPay.Exceptions;

public class OnvoPayException : Exception
{
    public int StatusCode { get; }
    public string? ApiCode { get; }
    public string? ApiMessage { get; }

    public OnvoPayException(int statusCode, string message, string? apiCode = null, string? apiMessage = null)
        : base(message)
    {
        StatusCode = statusCode;
        ApiCode = apiCode;
        ApiMessage = apiMessage;
    }
}
