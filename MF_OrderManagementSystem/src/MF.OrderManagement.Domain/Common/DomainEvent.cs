namespace MF.OrderManagement.Domain.Common;

public abstract record DomainEvent(Guid Id, DateTime OccurredAt)
{
    public static T Create<T>(Func<Guid, DateTime, T> factory)
        => factory(Guid.NewGuid(), DateTime.UtcNow);
}