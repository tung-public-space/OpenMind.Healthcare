using DDD.BuildingBlocks;

namespace UserApi.Domain.Events;

public class PasswordChangedEvent(Guid userId) : IDomainEvent
{
    public Guid UserId { get; } = userId;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
