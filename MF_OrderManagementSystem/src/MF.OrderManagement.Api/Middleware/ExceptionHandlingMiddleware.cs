using System.Net;
using MF.OrderManagement.Application.Common.Exceptions;
using MF.OrderManagement.Domain.Common;

namespace MF.OrderManagement.Api.Middleware;

public sealed class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            await WriteError(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (DomainException ex)
        {
            // regra de negócio / domínio -> 400
            await WriteError(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (ApplicationException ex)
        {
            await WriteError(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await WriteError(context, HttpStatusCode.InternalServerError, "Unexpected error.");
        }
    }

    private static async Task WriteError(HttpContext context, HttpStatusCode code, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        await context.Response.WriteAsJsonAsync(new
        {
            error = message,
            status = (int)code
        });
    }
}