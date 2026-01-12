using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Domain.ValueObjects;

namespace QuitSmokingApi.Domain.Services;

/// <summary>
/// Domain service for computing achievement statuses.
/// Achievement unlock status is derived from the journey's daysSmokeFree - no need to persist separately.
/// </summary>
public class AchievementStatusService
{
    /// <summary>
    /// Computes the achievement status for a single achievement based on the user's journey.
    /// </summary>
    public AchievementStatus ComputeStatus(Achievement achievement, QuitJourney? journey)
    {
        if (journey == null)
        {
            return AchievementStatus.CreateNotStarted(
                achievement.Id,
                achievement.Name,
                achievement.Description,
                achievement.Icon,
                achievement.RequiredDays,
                achievement.Category.ToString());
        }
        
        var daysSmokeFree = journey.GetDaysSmokeFree();
        
        return AchievementStatus.Create(
            achievement.Id,
            achievement.Name,
            achievement.Description,
            achievement.Icon,
            achievement.RequiredDays,
            achievement.Category.ToString(),
            daysSmokeFree,
            journey.QuitDate);
    }
    
    /// <summary>
    /// Computes achievement statuses for all achievements based on the user's journey.
    /// </summary>
    public IReadOnlyList<AchievementStatus> ComputeStatuses(
        IEnumerable<Achievement> achievements, 
        QuitJourney? journey)
    {
        return achievements
            .Select(a => ComputeStatus(a, journey))
            .ToList();
    }
}
