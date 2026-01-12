using DDD.BuildingBlocks;

namespace QuitSmokingApi.Domain.ValueObjects;

public class HealthMilestoneStatus : ValueObject
{
    public Guid MilestoneId { get; }
    public string Title { get; }
    public string Description { get; }
    public int TimeRequiredMinutes { get; }
    public string TimeDisplay { get; }
    public string Icon { get; }
    public string Category { get; }
    public bool IsAchieved { get; }
    public DateTime? AchievedAt { get; }
    public double ProgressPercentage { get; }
    
    private HealthMilestoneStatus(
        Guid milestoneId,
        string title,
        string description,
        int timeRequiredMinutes,
        string timeDisplay,
        string icon,
        string category,
        bool isAchieved,
        DateTime? achievedAt,
        double progressPercentage)
    {
        MilestoneId = milestoneId;
        Title = title;
        Description = description;
        TimeRequiredMinutes = timeRequiredMinutes;
        TimeDisplay = timeDisplay;
        Icon = icon;
        Category = category;
        IsAchieved = isAchieved;
        AchievedAt = achievedAt;
        ProgressPercentage = Math.Round(progressPercentage, 2);
    }
    
    public static HealthMilestoneStatus Create(
        Guid milestoneId,
        string title,
        string description,
        int timeRequiredMinutes,
        string timeDisplay,
        string icon,
        string category,
        int minutesSinceQuit,
        DateTime? quitDate)
    {
        var isAchieved = minutesSinceQuit >= timeRequiredMinutes;
        var progressPercentage = timeRequiredMinutes == 0 
            ? 100.0 
            : Math.Min(100, (double)minutesSinceQuit / timeRequiredMinutes * 100);
        
        DateTime? achievedAt = null;
        if (isAchieved && quitDate.HasValue)
        {
            achievedAt = quitDate.Value.AddMinutes(timeRequiredMinutes);
        }
        
        return new HealthMilestoneStatus(
            milestoneId,
            title,
            description,
            timeRequiredMinutes,
            timeDisplay,
            icon,
            category,
            isAchieved,
            achievedAt,
            progressPercentage);
    }
    
    public static HealthMilestoneStatus CreateNotStarted(
        Guid milestoneId,
        string title,
        string description,
        int timeRequiredMinutes,
        string timeDisplay,
        string icon,
        string category)
    {
        return new HealthMilestoneStatus(
            milestoneId,
            title,
            description,
            timeRequiredMinutes,
            timeDisplay,
            icon,
            category,
            isAchieved: false,
            achievedAt: null,
            progressPercentage: 0);
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return MilestoneId;
        yield return IsAchieved;
        yield return ProgressPercentage;
    }
}
