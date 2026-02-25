namespace MF.OrderManagement.Infrastructure.Auth;

public sealed class JwtOptions
{
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string SigningKey { get; init; } = string.Empty; // chave longa
    public int ExpirationMinutes { get; init; } = 60;
}