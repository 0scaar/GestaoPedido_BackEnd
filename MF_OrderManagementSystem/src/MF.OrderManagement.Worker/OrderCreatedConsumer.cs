using System.Text;
using System.Text.Json;
using MF.OrderManagement.Application.Orders.DTOs;
using MF.OrderManagement.Domain.Entities.Deliveries;
using MF.OrderManagement.Infrastructure.Messaging;
using MF.OrderManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MF.OrderManagement.Worker;

public class OrderCreatedConsumer: BackgroundService
{
    private readonly RabbitMqOptions _opt;
    private readonly IServiceScopeFactory _scopeFactory;

    private IConnection? _connection;
    private IModel? _channel;

    public OrderCreatedConsumer(IOptions<RabbitMqOptions> options, IServiceScopeFactory scopeFactory)
    {
        _opt = options.Value;
        _scopeFactory = scopeFactory;
    }
    
    public override Task StartAsync(CancellationToken cancellationToken)
    {
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

        _channel.ExchangeDeclare(exchange: _opt.Exchange, type: ExchangeType.Direct, durable: true, autoDelete: false);
        _channel.QueueDeclare(queue: _opt.Queue, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(queue: _opt.Queue, exchange: _opt.Exchange, routingKey: _opt.RoutingKey);

        // QoS: processa 1 por vez (mais seguro pra desafio)
        _channel.BasicQos(0, 1, false);

        return base.StartAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_channel is null) throw new InvalidOperationException("RabbitMQ channel not initialized.");

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += HandleMessageAsync;

        _channel.BasicConsume(
            queue: _opt.Queue,
            autoAck: false,
            consumer: consumer
        );

        return Task.CompletedTask;
    }
    
    private async Task HandleMessageAsync(object sender, BasicDeliverEventArgs ea)
    {
        if (_channel is null) return;

        try
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var msg = JsonSerializer.Deserialize<OrderCreatedMessage>(json);

            if (msg is null || msg.OrderId == Guid.Empty)
            {
                _channel.BasicAck(ea.DeliveryTag, multiple: false);
                return;
            }

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();

            // Busca pedido para pegar OrderDate
            var order = await db.Orders.FirstOrDefaultAsync(o => o.Id == msg.OrderId);
            if (order is null)
            {
                // nada pra fazer, ack
                _channel.BasicAck(ea.DeliveryTag, multiple: false);
                return;
            }

            // evita duplicar DeliveryTerms
            var exists = await db.DeliveryTerms.AnyAsync(x => x.OrderId == order.Id);
            if (!exists)
            {
                var deliveryDays = 10;
                var delivery = new DeliveryTerms
                (
                    id: Guid.NewGuid(),
                    orderId: order.Id,
                    estimatedDeliveryDate: order.OrderDate.AddDays(deliveryDays),
                    deliveryDays: deliveryDays
                );

                db.DeliveryTerms.Add(delivery);
                await db.SaveChangesAsync();
            }

            _channel.BasicAck(ea.DeliveryTag, multiple: false);
        }
        catch
        {
            // Requeue = true: tenta de novo (pra desafio ok)
            _channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        try { _channel?.Close(); } catch { }
        try { _connection?.Close(); } catch { }

        _channel?.Dispose();
        _connection?.Dispose();

        return base.StopAsync(cancellationToken);
    }
}