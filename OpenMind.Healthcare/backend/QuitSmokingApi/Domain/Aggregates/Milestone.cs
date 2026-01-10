using QuitSmokingApi.Domain.Common;

namespace QuitSmokingApi.Domain.Aggregates;

/// <summary>
/// Value object representing a milestone in the quit journey
/// </summary>
public class Milestone : ValueObject
{
    public string Name { get; }
    public string Description { get; }
    public int RequiredDays { get; }
    public string Icon { get; }
    
    private Milestone(string name, string description, int requiredDays, string icon)
    {
        Name = name;
        Description = description;
        RequiredDays = requiredDays;
        Icon = icon;
    }
    
    // Pre-defined milestones
    public static readonly Milestone FirstStep = new("First Step", "Started your quit smoking journey", 0, "🌟");
    public static readonly Milestone TwentyFourHours = new("24 Hours Strong", "Completed your first day smoke-free", 1, "🏅");
    public static readonly Milestone WeekendWarrior = new("Weekend Warrior", "Smoke-free for 3 days", 3, "💪");
    public static readonly Milestone OneWeek = new("One Week Wonder", "Completed one full week", 7, "🎯");
    public static readonly Milestone TwoWeeks = new("Two Week Champion", "Two weeks of freedom!", 14, "🏆");
    public static readonly Milestone ThreeWeeks = new("Three Week Hero", "21 days - a habit is forming!", 21, "⭐");
    public static readonly Milestone OneMonth = new("Monthly Master", "One full month smoke-free!", 30, "👑");
    public static readonly Milestone SixWeeks = new("Six Week Superstar", "42 days of determination", 42, "🌈");
    public static readonly Milestone TwoMonths = new("Two Month Titan", "60 days of pure willpower", 60, "🔥");
    public static readonly Milestone ThreeMonths = new("Quarter Year Champion", "90 days - you're unstoppable!", 90, "💎");
    public static readonly Milestone SixMonths = new("Half Year Hero", "180 days of freedom", 180, "🎊");
    public static readonly Milestone OneYear = new("Year One Legend", "365 days smoke-free!", 365, "🏰");
    
    private static readonly Milestone[] AllMilestones = {
        FirstStep, TwentyFourHours, WeekendWarrior, OneWeek, TwoWeeks,
        ThreeWeeks, OneMonth, SixWeeks, TwoMonths, ThreeMonths, SixMonths, OneYear
    };
    
    public static IReadOnlyList<Milestone> GetAll() => AllMilestones;
    
    public static Milestone GetMilestoneForDays(int days)
    {
        return AllMilestones
            .Where(m => days >= m.RequiredDays)
            .OrderByDescending(m => m.RequiredDays)
            .First();
    }
    
    public static Milestone? GetNextMilestone(int currentDays)
    {
        return AllMilestones
            .Where(m => m.RequiredDays > currentDays)
            .OrderBy(m => m.RequiredDays)
            .FirstOrDefault();
    }
    
    public bool IsAchieved(int currentDays) => currentDays >= RequiredDays;
    
    public int GetDaysRemaining(int currentDays) => Math.Max(0, RequiredDays - currentDays);
    
    public double GetProgress(int currentDays) => Math.Min(100, (double)currentDays / RequiredDays * 100);
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
        yield return RequiredDays;
    }
}
