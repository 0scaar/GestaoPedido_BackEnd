namespace MF.OrderManagement.Observability.Mongo;

public sealed class MongoDbOptions
{
    public string ConnectionString { get; init; } = string.Empty;
    public string DatabaseName { get; init; } = "orders_observability";
    public string LogsCollectionName { get; init; } = "application_logs";
}