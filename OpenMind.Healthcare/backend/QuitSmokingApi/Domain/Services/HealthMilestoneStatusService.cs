using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Domain.ValueObjects;

namespace QuitSmokingApi.Domain.Services;

public class HealthMilestoneStatusService
{
    public HealthMilestoneStatus ComputeStatus(HealthMilestone milestone, QuitJourney? journey)
    {
        if (journey == null)
        {
            return HealthMilestoneStatus.CreateNotStarted(
                milestone.Id,
                milestone.Title,
                milestone.Description,
                milestone.TimeRequiredMinutes,
                milestone.TimeDisplay,
                milestone.Icon,
                milestone.Category.ToString());
        }
        
        var minutesSinceQuit = (int)journey.GetTimeSinceQuit().TotalMinutes;
        
        return HealthMilestoneStatus.Create(
            milestone.Id,
            milestone.Title,
            milestone.Description,
            milestone.TimeRequiredMinutes,
            milestone.TimeDisplay,
            milestone.Icon,
            milestone.Category.ToString(),
            minutesSinceQuit,
            journey.QuitDate);
    }
    
    public IReadOnlyList<HealthMilestoneStatus> ComputeStatuses(
        IEnumerable<HealthMilestone> milestones, 
        QuitJourney? journey)
    {
        return milestones
            .Select(m => ComputeStatus(m, journey))
            .ToList();
    }
}
