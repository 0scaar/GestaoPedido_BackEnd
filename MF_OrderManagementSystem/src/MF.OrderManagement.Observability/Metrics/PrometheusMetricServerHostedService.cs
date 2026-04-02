using Microsoft.Extensions.Hosting;
using Prometheus;

namespace MF.OrderManagement.Observability.Metrics;

public sealed class PrometheusMetricServerHostedService(int port = 9091) : IHostedService, IDisposable
{
    private readonly KestrelMetricServer _server = new(port: port);

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _server.Start();
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _server.StopAsync();
    }

    public void Dispose()
    {
        _server.Dispose();
    }
}