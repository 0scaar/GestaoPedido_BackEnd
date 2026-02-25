using MF.OrderManagement.Domain.Common;

namespace MF.OrderManagement.Domain.Entities.Orders.Events;

public record OrderCreatedDomainEvent(
    Guid Id,
    DateTime OccurredAtUtc,
    Guid OrderId
) : DomainEvent(Id, OccurredAtUtc);