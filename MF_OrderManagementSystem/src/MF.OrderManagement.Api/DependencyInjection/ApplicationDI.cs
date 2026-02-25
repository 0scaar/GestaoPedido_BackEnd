using MF.OrderManagement.Application.Orders.UseCases.ApproveOrder;
using MF.OrderManagement.Application.Orders.UseCases.CreateOrder;
using MF.OrderManagement.Application.Orders.UseCases.GetOrders;

namespace MF.OrderManagement.Api.DependencyInjection;

public static class ApplicationDI
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateOrderUseCase>();
        services.AddScoped<ApproveOrderUseCase>();
        services.AddScoped<GetOrdersUseCase>();

        return services;
    }
}