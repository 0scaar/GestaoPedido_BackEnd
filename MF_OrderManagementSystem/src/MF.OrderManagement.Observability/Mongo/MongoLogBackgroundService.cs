using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MF.OrderManagement.Observability.Mongo;

public sealed class MongoLogBackgroundService : BackgroundService
{
    private readonly MongoLogChannel _channel;
    private readonly IMongoCollection<MongoLogDocument> _collection;

    public MongoLogBackgroundService(
        MongoLogChannel channel,
        IMongoClient mongoClient,
        IOptions<MongoDbOptions> options)
    {
        _channel = channel;

        var database = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = database.GetCollection<MongoLogDocument>(options.Value.LogsCollectionName);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var batch = new List<MongoLogDocument>(50);

        while (!stoppingToken.IsCancellationRequested)
        {
            batch.Clear();

            while (batch.Count < 50 && await _channel.Channel.Reader.WaitToReadAsync(stoppingToken))
            {
                while (batch.Count < 50 && _channel.Channel.Reader.TryRead(out var item))
                {
                    batch.Add(item);
                }

                if (batch.Count > 0)
                    break;
            }

            if (batch.Count == 0)
                continue;

            try
            {
                if (batch.Count == 1)
                {
                    await _collection.InsertOneAsync(batch[0], cancellationToken: stoppingToken);
                }
                else
                {
                    await _collection.InsertManyAsync(batch, cancellationToken: stoppingToken);
                }
            }
            catch
            {
                // Aquí evitamos romper la app por un problema de Mongo.
                // Si quieres, luego añadimos retry/backoff y fallback a archivo.
            }
        }
    }
}