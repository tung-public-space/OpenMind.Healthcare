namespace QuitSmokingApi.Features.Progress.GetHealthMilestones;

/// <summary>
/// DTO for health milestone response - maps from domain value object
/// </summary>
public record HealthMilestoneDto(
    Guid Id,
    string Title,
    string Description,
    int TimeInMinutes,
    string TimeDisplay,
    string Icon,
    string Category,
    bool IsAchieved,
    double ProgressPercentage
);
