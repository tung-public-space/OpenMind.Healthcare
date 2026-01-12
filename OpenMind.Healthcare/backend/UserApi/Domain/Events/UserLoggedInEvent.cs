using DDD.BuildingBlocks;

namespace UserApi.Domain.Events;

public class UserLoggedInEvent(Guid userId, DateTime loginTime) : IDomainEvent
{
    public Guid UserId { get; } = userId;
    public DateTime LoginTime { get; } = loginTime;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
