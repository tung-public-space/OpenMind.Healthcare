using DDD.BuildingBlocks;

namespace QuitSmokingApi.Domain.Events;

/// <summary>
/// Domain event raised when a quit journey is updated
/// </summary>
public class JourneyUpdatedEvent(Guid journeyId, DateTime newQuitDate) : IDomainEvent
{
    public Guid JourneyId { get; } = journeyId;
    public DateTime NewQuitDate { get; } = newQuitDate;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
