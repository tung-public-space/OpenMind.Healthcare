using DDD.BuildingBlocks;

namespace UserApi.Domain.Events;

public class UserRegisteredEvent(Guid userId, string email) : IDomainEvent
{
    public Guid UserId { get; } = userId;
    public string Email { get; } = email;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
