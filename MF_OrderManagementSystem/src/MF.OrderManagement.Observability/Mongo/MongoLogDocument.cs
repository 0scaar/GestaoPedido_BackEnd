using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MF.OrderManagement.Observability.Mongo;

public sealed class MongoLogDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public DateTime TimestampUtc { get; set; }
    public string Level { get; set; } = string.Empty;
    public string MessageTemplate { get; set; } = string.Empty;
    public string RenderedMessage { get; set; } = string.Empty;
    public string? Exception { get; set; }
    public string Application { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
    public string? TraceId { get; set; }
    public string? SpanId { get; set; }

    public Dictionary<string, object?> Properties { get; set; } = new();
}