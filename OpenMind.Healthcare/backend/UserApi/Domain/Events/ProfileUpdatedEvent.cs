using DDD.BuildingBlocks;

namespace UserApi.Domain.Events;

public class ProfileUpdatedEvent(Guid userId, string firstName, string lastName) : IDomainEvent
{
    public Guid UserId { get; } = userId;
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
