namespace Shared.Response;

public class TokenValidationResponse
{
    public string Message { get; set; } = "";
    public string User { get; set; } = "";
    public double TimeRemaining { get; set; } // Tiempo en segundos
}