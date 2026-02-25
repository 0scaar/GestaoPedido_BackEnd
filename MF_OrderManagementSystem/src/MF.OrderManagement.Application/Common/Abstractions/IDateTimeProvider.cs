namespace MF.OrderManagement.Application.Common.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}