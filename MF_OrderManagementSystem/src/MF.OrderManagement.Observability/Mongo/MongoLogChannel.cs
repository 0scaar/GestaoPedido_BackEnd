using System.Threading.Channels;

namespace MF.OrderManagement.Observability.Mongo;

public sealed class MongoLogChannel
{
    public Channel<MongoLogDocument> Channel { get; } =
        System.Threading.Channels.Channel.CreateUnbounded<MongoLogDocument>(
            new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });
}