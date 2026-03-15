namespace MF.OrderManagement.Infrastructure.Messaging;

public sealed class RabbitMqOptions
{
    public string Host { get; init; } = string.Empty;
    public int Port { get; init; } = 5672;
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;

    public string Exchange { get; init; } = string.Empty;
    public string Queue { get; init; } = string.Empty;
    public string RoutingKey { get; init; } = string.Empty;
}