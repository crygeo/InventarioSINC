namespace Shared.ObjectsResponse;

public class ErrorResponse
{
    public ErrorResponse(int statusCode, string message, string? error = null)
    {
        StatusCode = statusCode;
        Message = message;
        Error = error ?? "";
    }

    public ErrorResponse(string message, string? error = null)
    {
        Message = message;
        Error = error ?? "";
    }

    public ErrorResponse()
    {
    }

    public string Message { get; init; } = string.Empty;
    public string Error { get; init; } = string.Empty;
    public int StatusCode { get; init; }
}