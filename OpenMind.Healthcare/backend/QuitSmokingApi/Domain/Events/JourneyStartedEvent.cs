using QuitSmokingApi.Domain.Common;

namespace QuitSmokingApi.Domain.Events;

/// <summary>
/// Domain event raised when a new quit journey is started
/// </summary>
public class JourneyStartedEvent : IDomainEvent
{
    public Guid JourneyId { get; }
    public DateTime QuitDate { get; }
    public DateTime OccurredOn { get; }
    
    public JourneyStartedEvent(Guid journeyId, DateTime quitDate)
    {
        JourneyId = journeyId;
        QuitDate = quitDate;
        OccurredOn = DateTime.UtcNow;
    }
}
