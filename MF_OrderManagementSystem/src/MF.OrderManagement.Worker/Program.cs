using MF.OrderManagement.Infrastructure.Messaging;
using MF.OrderManagement.Infrastructure.Persistence;
using MF.OrderManagement.Worker;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddDbContext<OrdersDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("OrdersDb")));

builder.Services.AddHostedService<OrderCreatedConsumer>();

var host = builder.Build();
host.Run();