using QuitSmokingApi.Domain.Common;

namespace QuitSmokingApi.Domain.Events;

/// <summary>
/// Domain event raised when a quit journey is updated
/// </summary>
public class JourneyUpdatedEvent : IDomainEvent
{
    public Guid JourneyId { get; }
    public DateTime NewQuitDate { get; }
    public DateTime OccurredOn { get; }
    
    public JourneyUpdatedEvent(Guid journeyId, DateTime newQuitDate)
    {
        JourneyId = journeyId;
        NewQuitDate = newQuitDate;
        OccurredOn = DateTime.UtcNow;
    }
}
