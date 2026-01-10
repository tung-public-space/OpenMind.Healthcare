using QuitSmokingApi.Domain.Common;

namespace QuitSmokingApi.Domain.Events;

/// <summary>
/// Domain event raised when a new achievement is unlocked
/// </summary>
public class AchievementUnlockedEvent : IDomainEvent
{
    public Guid JourneyId { get; }
    public string AchievementName { get; }
    public int RequiredDays { get; }
    public DateTime OccurredOn { get; }
    
    public AchievementUnlockedEvent(Guid journeyId, string achievementName, int requiredDays)
    {
        JourneyId = journeyId;
        AchievementName = achievementName;
        RequiredDays = requiredDays;
        OccurredOn = DateTime.UtcNow;
    }
}
