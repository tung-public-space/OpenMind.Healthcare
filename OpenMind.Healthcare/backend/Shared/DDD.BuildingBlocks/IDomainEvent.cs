using MediatR;

namespace DDD.BuildingBlocks;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
