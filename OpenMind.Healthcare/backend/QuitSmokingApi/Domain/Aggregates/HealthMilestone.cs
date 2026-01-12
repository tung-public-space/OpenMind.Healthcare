using DDD.BuildingBlocks;

namespace QuitSmokingApi.Domain.Aggregates;

public class HealthMilestone : AggregateRoot
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int TimeRequiredMinutes { get; private set; }
    public string TimeDisplay { get; private set; } = string.Empty;
    public string Icon { get; private set; } = string.Empty;
    public HealthCategory Category { get; private set; }
    
    private HealthMilestone() { }
    
    private HealthMilestone(string title, string description, int timeRequiredMinutes, string timeDisplay, string icon, HealthCategory category)
    {
        Title = title;
        Description = description;
        TimeRequiredMinutes = timeRequiredMinutes;
        TimeDisplay = timeDisplay;
        Icon = icon;
        Category = category;
    }
    
    public static HealthMilestone Create(string title, string description, int timeRequiredMinutes, string timeDisplay, string icon, HealthCategory category)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Health milestone title is required");
            
        if (timeRequiredMinutes < 0)
            throw new DomainException("Time required cannot be negative");
            
        return new HealthMilestone(title, description, timeRequiredMinutes, timeDisplay, icon, category);
    }
    
    public bool IsAchievedFor(int minutesSinceQuit) => minutesSinceQuit >= TimeRequiredMinutes;
    
    public double GetProgress(int minutesSinceQuit)
    {
        if (TimeRequiredMinutes == 0) return 100;
        return Math.Min(100, (double)minutesSinceQuit / TimeRequiredMinutes * 100);
    }
}

public enum HealthCategory
{
    Cardiovascular,
    Respiratory,
    Sensory,
    Energy,
    CancerPrevention
}
