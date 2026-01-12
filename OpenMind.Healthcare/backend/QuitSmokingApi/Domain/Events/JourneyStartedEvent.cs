using DDD.BuildingBlocks;

namespace QuitSmokingApi.Domain.Events;

/// <summary>
/// Domain event raised when a new quit journey is started
/// </summary>
public class JourneyStartedEvent(Guid journeyId, DateTime quitDate) : IDomainEvent
{
    public Guid JourneyId { get; } = journeyId;
    public DateTime QuitDate { get; } = quitDate;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
