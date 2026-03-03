using FluentValidation;
using MF.OrderManagement.Application.Orders.DTOs;
using MF.OrderManagement.Application.Orders.UseCases.ApproveOrder;
using MF.OrderManagement.Application.Orders.UseCases.CreateOrder;
using MF.OrderManagement.Application.Orders.UseCases.GetOrders;
using MF.OrderManagement.Application.Orders.Validators;

namespace MF.OrderManagement.Api.DependencyInjection;

public static class ApplicationDI
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateOrderUseCase>();
        services.AddScoped<ApproveOrderUseCase>();
        services.AddScoped<GetOrdersUseCase>();
        services.AddScoped<GetOrderByIdUseCase>();

        services.AddScoped<IValidator<CreateOrderRequest>, CreateOrderRequestValidator>();

        return services;
    }
}