namespace MF.OrderManagement.Infrastructure.Messaging;

public sealed class RabbitMqOptions
{
    public string Host { get; init; } = "localhost";
    public int Port { get; init; } = 5672;
    public string Username { get; init; } = "guest";
    public string Password { get; init; } = "guest";

    public string Exchange { get; init; } = "orders";
    public string Queue { get; init; } = "orders.order-created";
    public string RoutingKey { get; init; } = "orders.order-created";
}