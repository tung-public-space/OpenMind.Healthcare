using QuitSmokingApi.Domain.Common;

namespace QuitSmokingApi.Domain.Aggregates;

/// <summary>
/// Aggregate root for achievements that can be earned during the quit journey
/// </summary>
public class Achievement : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Icon { get; private set; } = string.Empty;
    public int RequiredDays { get; private set; }
    public AchievementCategory Category { get; private set; }
    
    // Private constructor for EF Core
    private Achievement() { }
    
    private Achievement(string name, string description, string icon, int requiredDays, AchievementCategory category)
    {
        Name = name;
        Description = description;
        Icon = icon;
        RequiredDays = requiredDays;
        Category = category;
    }
    
    public static Achievement Create(string name, string description, string icon, int requiredDays, AchievementCategory category)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Achievement name is required");
            
        if (requiredDays < 0)
            throw new DomainException("Required days cannot be negative");
            
        return new Achievement(name, description, icon, requiredDays, category);
    }
    
    public bool IsUnlockedFor(int daysSmokeFree) => daysSmokeFree >= RequiredDays;
    
    public bool IsExactlyUnlockedFor(int daysSmokeFree) => daysSmokeFree == RequiredDays;
    
    public double GetProgress(int daysSmokeFree)
    {
        if (RequiredDays == 0) return 100;
        return Math.Min(100, (double)daysSmokeFree / RequiredDays * 100);
    }
}

public enum AchievementCategory
{
    Milestone,
    Health,
    Financial,
    Social
}
