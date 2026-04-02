using Serilog.Core;
using Serilog.Events;

namespace MF.OrderManagement.Observability.Mongo;

public sealed class MongoDbLogSink(MongoLogChannel channel, string application, string environment) : ILogEventSink
{
    public void Emit(LogEvent logEvent)
    {
        var properties = new Dictionary<string, object?>();

        foreach (var kvp in logEvent.Properties)
        {
            properties[kvp.Key] = Simplify(kvp.Value);
        }

        var doc = new MongoLogDocument
        {
            TimestampUtc = logEvent.Timestamp.UtcDateTime,
            Level = logEvent.Level.ToString(),
            MessageTemplate = logEvent.MessageTemplate.Text,
            RenderedMessage = logEvent.RenderMessage(),
            Exception = logEvent.Exception?.ToString(),
            Application = application,
            Environment = environment,
            TraceId = TryGetScalar(logEvent, "TraceId"),
            SpanId = TryGetScalar(logEvent, "SpanId"),
            Properties = properties
        };

        channel.Channel.Writer.TryWrite(doc);
    }
    
    private static string? TryGetScalar(LogEvent logEvent, string name)
    {
        if (logEvent.Properties.TryGetValue(name, out var value) &&
            value is ScalarValue scalar &&
            scalar.Value is not null)
        {
            return scalar.Value.ToString();
        }

        return null;
    }

    private static object? Simplify(LogEventPropertyValue value)
    {
        return value switch
        {
            ScalarValue s => s.Value,
            SequenceValue seq => seq.Elements.Select(Simplify).ToArray(),
            StructureValue st => st.Properties.ToDictionary(p => p.Name, p => Simplify(p.Value)),
            DictionaryValue dv => dv.Elements.ToDictionary(
                k => k.Key.Value?.ToString() ?? string.Empty,
                v => Simplify(v.Value)),
            _ => value.ToString()
        };
    }
}