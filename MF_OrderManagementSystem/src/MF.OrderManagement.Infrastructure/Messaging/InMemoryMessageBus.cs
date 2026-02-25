using MF.OrderManagement.Application.Common.Abstractions;

namespace MF.OrderManagement.Infrastructure.Messaging;

public class InMemoryMessageBus : IMessageBus
{
    public readonly List<object> PublishedMessages = new();

    public Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class
    {
        PublishedMessages.Add(message);
        return Task.CompletedTask;
    }
}