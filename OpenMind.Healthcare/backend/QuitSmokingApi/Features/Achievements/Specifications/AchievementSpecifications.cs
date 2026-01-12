using System.Linq.Expressions;
using DDD.BuildingBlocks.Specifications;

namespace QuitSmokingApi.Features.Achievements.Specifications;

/// <summary>
/// Application layer specification for filtering unlocked achievements
/// </summary>
public class UnlockedAchievementSpecification : Specification<AchievementDto>
{
    public override string RuleDescription => "Achievement is unlocked";
    
    public override Expression<Func<AchievementDto, bool>> ToExpression() 
        => achievement => achievement.IsUnlocked;
}

/// <summary>
/// Application layer specification for filtering locked achievements
/// </summary>
public class LockedAchievementSpecification : Specification<AchievementDto>
{
    public override string RuleDescription => "Achievement is locked";
    
    public override Expression<Func<AchievementDto, bool>> ToExpression() 
        => achievement => !achievement.IsUnlocked;
}

/// <summary>
/// Application layer specification for filtering achievements by category
/// </summary>
public class AchievementByCategorySpecification : Specification<AchievementDto>
{
    private readonly string _category;
    
    public AchievementByCategorySpecification(string category)
    {
        _category = category;
    }
    
    public override string RuleDescription => $"Achievement belongs to category '{_category}'";
    
    public override Expression<Func<AchievementDto, bool>> ToExpression() 
        => achievement => achievement.Category == _category;
}

/// <summary>
/// Application layer specification for filtering achievements with progress above threshold
/// </summary>
public class AchievementProgressAboveSpecification : Specification<AchievementDto>
{
    private readonly double _threshold;
    
    public AchievementProgressAboveSpecification(double threshold)
    {
        _threshold = threshold;
    }
    
    public override string RuleDescription => $"Achievement progress above {_threshold}%";
    
    public override Expression<Func<AchievementDto, bool>> ToExpression() 
        => achievement => achievement.ProgressPercentage >= _threshold;
}

/// <summary>
/// Application layer specification for filtering achievements achievable within days
/// </summary>
public class AchievementWithinDaysSpecification(int maxDays) : Specification<AchievementDto>
{
    public override string RuleDescription => $"Achievement achievable within {maxDays} days";
    
    public override Expression<Func<AchievementDto, bool>> ToExpression() 
        => achievement => achievement.RequiredDays <= maxDays;
}

/// <summary>
/// Factory for creating common achievement specifications
/// </summary>
public static class AchievementSpecs
{
    public static UnlockedAchievementSpecification Unlocked() => new();
    public static LockedAchievementSpecification Locked() => new();
    public static AchievementByCategorySpecification ByCategory(string category) => new(category);
    public static AchievementProgressAboveSpecification ProgressAbove(double threshold) => new(threshold);
    public static AchievementWithinDaysSpecification WithinDays(int maxDays) => new(maxDays);
    
    /// <summary>
    /// Achievements that are close to being unlocked (50%+ progress but not yet unlocked)
    /// </summary>
    public static Specification<AchievementDto> AlmostUnlocked() 
        => ProgressAbove(50) & Locked();
    
    /// <summary>
    /// Milestone achievements that are unlocked
    /// </summary>
    public static Specification<AchievementDto> UnlockedMilestones() 
        => Unlocked() & ByCategory("Milestone");
}
