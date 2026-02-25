using System.Text;
using System.Text.Json;
using MF.OrderManagement.Application.Common.Abstractions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MF.OrderManagement.Infrastructure.Messaging;

public sealed class RabbitMqMessageBus : IMessageBus, IDisposable
{
    private readonly RabbitMqOptions _opt;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    
    public RabbitMqMessageBus(IOptions<RabbitMqOptions> options)
    {
        _opt = options.Value;

        var factory = new ConnectionFactory
        {
            HostName = _opt.Host,
            Port = _opt.Port,
            UserName = _opt.Username,
            Password = _opt.Password,
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // exchange + queue + bind
        _channel.ExchangeDeclare(exchange: _opt.Exchange, type: ExchangeType.Direct, durable: true, autoDelete: false);
        _channel.QueueDeclare(queue: _opt.Queue, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(queue: _opt.Queue, exchange: _opt.Exchange, routingKey: _opt.RoutingKey);
    }
    
    public Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var props = _channel.CreateBasicProperties();
        props.Persistent = true; // mensagem persistente

        _channel.BasicPublish(
            exchange: _opt.Exchange,
            routingKey: _opt.RoutingKey,
            basicProperties: props,
            body: body
        );

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        try { _channel.Close(); } catch { }
        try { _connection.Close(); } catch { }
        _channel.Dispose();
        _connection.Dispose();
    }
}