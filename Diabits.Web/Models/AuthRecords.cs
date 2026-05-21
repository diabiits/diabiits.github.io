namespace Diabits.Web.Models;

public record AuthResponse(string AccessToken);

public record AuthResult(bool Ok, string? Error)
{
    public static AuthResult Success() => new(true, null);
    public static AuthResult Fail(string error) => new(false, error);
}