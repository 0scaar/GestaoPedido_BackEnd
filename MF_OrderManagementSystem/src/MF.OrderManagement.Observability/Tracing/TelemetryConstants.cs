using System.Diagnostics;

namespace MF.OrderManagement.Observability.Tracing;

public class TelemetryConstants
{
    public const string ApiServiceName = "orders-api";
    public const string WorkerServiceName = "orders-worker";
    public const string ActivitySourceName = "MF.OrderManagement";

    public static readonly ActivitySource ActivitySource = new(ActivitySourceName);
}