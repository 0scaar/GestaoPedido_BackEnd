using MF.OrderManagement.Application.Common.Abstractions;
using MF.OrderManagement.Application.Orders.Ports;
using MF.OrderManagement.Infrastructure.Auth;
using MF.OrderManagement.Infrastructure.Common;
using MF.OrderManagement.Infrastructure.Messaging;
using MF.OrderManagement.Infrastructure.Persistence;
using MF.OrderManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MF.OrderManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<OrdersDbContext>(opt =>
            opt.UseSqlServer(config.GetConnectionString("OrdersDb")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IPaymentConditionRepository, PaymentConditionRepository>();
        
        services.Configure<JwtOptions>(config.GetSection("Jwt"));
        services.AddSingleton<ITokenService, JwtTokenService>();

        // Mensageria (trocar depois por RabbitMQ)
        //services.AddSingleton<IMessageBus, InMemoryMessageBus>();
        services.Configure<RabbitMqOptions>(config.GetSection("RabbitMq"));
        services.AddSingleton<IMessageBus, RabbitMqMessageBus>();

        return services;
    }
}