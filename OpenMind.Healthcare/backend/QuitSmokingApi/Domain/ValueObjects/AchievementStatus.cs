using DDD.BuildingBlocks;

namespace QuitSmokingApi.Domain.ValueObjects;

/// <summary>
/// Value object representing the status of an achievement for a specific user.
/// Encapsulates the logic for determining if an achievement is unlocked and progress calculation.
/// </summary>
public class AchievementStatus : ValueObject
{
    public Guid AchievementId { get; }
    public string Name { get; }
    public string Description { get; }
    public string Icon { get; }
    public int RequiredDays { get; }
    public string Category { get; }
    public bool IsUnlocked { get; }
    public DateTime? UnlockedAt { get; }
    public double ProgressPercentage { get; }
    
    private AchievementStatus(
        Guid achievementId,
        string name,
        string description,
        string icon,
        int requiredDays,
        string category,
        bool isUnlocked,
        DateTime? unlockedAt,
        double progressPercentage)
    {
        AchievementId = achievementId;
        Name = name;
        Description = description;
        Icon = icon;
        RequiredDays = requiredDays;
        Category = category;
        IsUnlocked = isUnlocked;
        UnlockedAt = unlockedAt;
        ProgressPercentage = Math.Round(progressPercentage, 2);
    }
    
    /// <summary>
    /// Creates an achievement status based on days smoke-free.
    /// The unlock logic is encapsulated here rather than in the handler.
    /// </summary>
    public static AchievementStatus Create(
        Guid achievementId,
        string name,
        string description,
        string icon,
        int requiredDays,
        string category,
        int daysSmokeFree,
        DateTime? quitDate)
    {
        var isUnlocked = daysSmokeFree >= requiredDays;
        var progressPercentage = requiredDays == 0 
            ? 100.0 
            : Math.Min(100, (double)daysSmokeFree / requiredDays * 100);
        
        DateTime? unlockedAt = null;
        if (isUnlocked && quitDate.HasValue)
        {
            unlockedAt = quitDate.Value.AddDays(requiredDays);
        }
        
        return new AchievementStatus(
            achievementId,
            name,
            description,
            icon,
            requiredDays,
            category,
            isUnlocked,
            unlockedAt,
            progressPercentage);
    }
    
    /// <summary>
    /// Creates an achievement status when user has no journey (not started yet).
    /// </summary>
    public static AchievementStatus CreateNotStarted(
        Guid achievementId,
        string name,
        string description,
        string icon,
        int requiredDays,
        string category)
    {
        return new AchievementStatus(
            achievementId,
            name,
            description,
            icon,
            requiredDays,
            category,
            isUnlocked: false,
            unlockedAt: null,
            progressPercentage: 0);
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return AchievementId;
        yield return IsUnlocked;
        yield return ProgressPercentage;
    }
}
