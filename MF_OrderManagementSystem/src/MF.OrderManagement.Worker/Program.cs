using MF.OrderManagement.Infrastructure.Messaging;
using MF.OrderManagement.Infrastructure.Persistence;
using MF.OrderManagement.Observability.Metrics;
using MF.OrderManagement.Observability.Mongo;
using MF.OrderManagement.Observability.Tracing;
using MF.OrderManagement.Worker;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

const string appName = "orders-worker";

// Mongo options
builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection("MongoDb"));
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<MongoDbOptions>>().Value;
    return new MongoClient(options.ConnectionString);
});
builder.Services.AddSingleton<MongoLogChannel>();
builder.Services.AddHostedService<MongoLogBackgroundService>();

// Serilog
builder.Services.AddSerilog((services, configuration) =>
{
    var env = builder.Environment.EnvironmentName;
    var channel = services.GetRequiredService<MongoLogChannel>();

    configuration
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", appName)
        .Enrich.WithProperty("Environment", env)
        .WriteTo.Console()
        .WriteTo.Sink(new MongoDbLogSink(channel, appName, env));
});

// OpenTelemetry traces
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName: appName))
    .WithTracing(tracing =>
    {
        tracing
            .AddSource(TelemetryConstants.ActivitySourceName)
            .AddHttpClientInstrumentation()
            .AddOtlpExporter();
    });

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddDbContext<OrdersDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("OrdersDb")));

builder.Services.AddHostedService<OrderCreatedConsumer>();

// Prometheus standalone server en :9091/metrics
builder.Services.AddSingleton<IHostedService>(_ => new PrometheusMetricServerHostedService(9091));

var host = builder.Build();
host.Run();