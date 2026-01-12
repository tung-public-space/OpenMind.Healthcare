using DDD.BuildingBlocks;
using QuitSmokingApi.Domain.Events;
using QuitSmokingApi.Domain.ValueObjects;

namespace QuitSmokingApi.Domain.Aggregates;

/// <summary>
/// Aggregate root representing a user's quit smoking journey
/// Contains all behavior and business logic related to quitting smoking
/// </summary>
public class QuitJourney : AggregateRoot
{
    // Constants for health calculations
    private const int MinutesOfLifePerCigarette = 11;
    
    public Guid UserId { get; private set; }
    public DateTime QuitDate { get; private set; }
    public SmokingHabits SmokingHabits { get; private set; } = null!;
    
    // Private constructor for EF Core
    private QuitJourney() { }
    
    private QuitJourney(Guid userId, DateTime quitDate, SmokingHabits smokingHabits)
    {
        UserId = userId;
        QuitDate = quitDate;
        SmokingHabits = smokingHabits;
        
        AddDomainEvent(new JourneyStartedEvent(Id, quitDate));
    }
    
    /// <summary>
    /// Factory method to start a new quit journey
    /// </summary>
    public static QuitJourney Start(Guid userId, DateTime quitDate, int cigarettesPerDay, int cigarettesPerPack, decimal pricePerPack)
    {
        if (quitDate > DateTime.UtcNow)
            throw new DomainException("Quit date cannot be in the future");
            
        var habits = SmokingHabits.Create(cigarettesPerDay, cigarettesPerPack, pricePerPack);
        return new QuitJourney(userId, quitDate, habits);
    }
    
    /// <summary>
    /// Update the journey details
    /// </summary>
    public void Update(DateTime quitDate, int cigarettesPerDay, int cigarettesPerPack, decimal pricePerPack)
    {
        if (quitDate > DateTime.UtcNow)
            throw new DomainException("Quit date cannot be in the future");
            
        QuitDate = quitDate;
        SmokingHabits = SmokingHabits.Create(cigarettesPerDay, cigarettesPerPack, pricePerPack);
        SetUpdated();
        
        AddDomainEvent(new JourneyUpdatedEvent(Id, quitDate));
    }
    
    /// <summary>
    /// Calculate the duration since quit date
    /// </summary>
    public Duration GetTimeSinceQuit(DateTime? asOf = null)
    {
        var now = asOf ?? DateTime.UtcNow;
        var timeSpan = now - QuitDate;
        return Duration.FromMinutes((int)timeSpan.TotalMinutes);
    }
    
    /// <summary>
    /// Calculate number of days smoke-free
    /// </summary>
    public int GetDaysSmokeFree(DateTime? asOf = null)
    {
        var now = asOf ?? DateTime.UtcNow;
        return (int)(now - QuitDate).TotalDays;
    }
    
    /// <summary>
    /// Calculate cigarettes not smoked
    /// </summary>
    public int GetCigarettesAvoided(DateTime? asOf = null)
    {
        var duration = GetTimeSinceQuit(asOf);
        var days = duration.TotalMinutes / (24.0 * 60.0);
        return (int)(days * SmokingHabits.CigarettesPerDay);
    }
    
    /// <summary>
    /// Calculate money saved
    /// </summary>
    public Money GetMoneySaved(DateTime? asOf = null)
    {
        var cigarettesAvoided = GetCigarettesAvoided(asOf);
        return SmokingHabits.PricePerCigarette.Multiply(cigarettesAvoided);
    }
    
    /// <summary>
    /// Calculate life regained based on cigarettes not smoked
    /// </summary>
    public Duration GetLifeRegained(DateTime? asOf = null)
    {
        var cigarettesAvoided = GetCigarettesAvoided(asOf);
        return Duration.FromMinutes(cigarettesAvoided * MinutesOfLifePerCigarette);
    }
    
    /// <summary>
    /// Get the current milestone based on days smoke-free
    /// </summary>
    public Milestone GetCurrentMilestone(DateTime? asOf = null)
    {
        var days = GetDaysSmokeFree(asOf);
        return Milestone.GetMilestoneForDays(days);
    }
    
    /// <summary>
    /// Get the next milestone to achieve
    /// </summary>
    public Milestone? GetNextMilestone(DateTime? asOf = null)
    {
        var days = GetDaysSmokeFree(asOf);
        return Milestone.GetNextMilestone(days);
    }
    
    /// <summary>
    /// Calculate progress percentage towards one year milestone
    /// </summary>
    public double GetProgressPercentage(DateTime? asOf = null)
    {
        var days = GetDaysSmokeFree(asOf);
        return Math.Min(100, (double)days / 365 * 100);
    }
    
    /// <summary>
    /// Generate complete progress statistics
    /// </summary>
    public ProgressStatistics GetStatistics(DateTime? asOf = null)
    {
        var duration = GetTimeSinceQuit(asOf);
        var daysSmokeFree = GetDaysSmokeFree(asOf);
        var currentMilestone = GetCurrentMilestone(asOf);
        var nextMilestone = GetNextMilestone(asOf);
        
        return new ProgressStatistics(
            daysSmokeFree: daysSmokeFree,
            hoursSmokeFree: duration.Hours,
            minutesSmokeFree: duration.TotalMinutes,
            cigarettesAvoided: GetCigarettesAvoided(asOf),
            moneySaved: GetMoneySaved(asOf),
            lifeRegained: GetLifeRegained(asOf),
            progressPercentage: GetProgressPercentage(asOf),
            currentMilestone: currentMilestone,
            nextMilestone: nextMilestone,
            daysToNextMilestone: nextMilestone?.GetDaysRemaining(daysSmokeFree) ?? 0
        );
    }
}
