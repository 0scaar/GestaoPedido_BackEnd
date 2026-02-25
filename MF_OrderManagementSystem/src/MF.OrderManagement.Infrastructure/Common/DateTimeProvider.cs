using MF.OrderManagement.Application.Common.Abstractions;

namespace MF.OrderManagement.Infrastructure.Common;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}