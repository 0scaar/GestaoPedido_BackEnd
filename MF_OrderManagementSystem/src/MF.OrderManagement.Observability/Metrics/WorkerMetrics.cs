using Prometheus;

namespace MF.OrderManagement.Observability.Metrics;

public static class WorkerMetrics
{
    public static readonly Counter MessagesConsumed = Prometheus.Metrics.CreateCounter(
        "orders_worker_messages_consumed_total",
        "Total de mensajes consumidos por el worker.");

    public static readonly Counter MessagesFailed = Prometheus.Metrics.CreateCounter(
        "orders_worker_messages_failed_total",
        "Total de mensajes fallidos en el worker.");

    public static readonly Counter DeliveryTermsCreated = Prometheus.Metrics.CreateCounter(
        "orders_worker_delivery_terms_created_total",
        "Total de delivery terms creados por el worker.");

    public static readonly Histogram ProcessingDuration = Prometheus.Metrics.CreateHistogram(
        "orders_worker_processing_duration_seconds",
        "Duración del procesamiento de mensajes del worker.");
}