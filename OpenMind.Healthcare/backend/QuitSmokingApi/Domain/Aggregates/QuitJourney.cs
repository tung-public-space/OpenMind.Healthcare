using DDD.BuildingBlocks;
using QuitSmokingApi.Domain.Events;
using QuitSmokingApi.Domain.Specifications;
using QuitSmokingApi.Domain.ValueObjects;

namespace QuitSmokingApi.Domain.Aggregates;

public class QuitJourney : AggregateRoot
{
    private const int MinutesOfLifePerCigarette = 11;
    
    public Guid UserId { get; private set; }
    public DateTime QuitDate { get; private set; }
    public SmokingHabits SmokingHabits { get; private set; } = null!;
    
    private QuitJourney() { }
    
    private QuitJourney(Guid userId, DateTime quitDate, SmokingHabits smokingHabits)
    {
        UserId = userId;
        QuitDate = quitDate;
        SmokingHabits = smokingHabits;
        
        AddDomainEvent(new JourneyStartedEvent(Id, quitDate));
    }
    
    public static QuitJourney Start(Guid userId, DateTime quitDate, int cigarettesPerDay, int cigarettesPerPack, decimal pricePerPack)
    {
        var userIdSpec = QuitJourneySpecs.UserIdNotEmpty();
        if (!userIdSpec.IsSatisfiedBy(userId))
            throw new DomainException(userIdSpec.RuleDescription);
        
        var quitDateSpec = QuitJourneySpecs.ValidQuitDate();
        if (!quitDateSpec.IsSatisfiedBy(quitDate))
            throw new DomainException(quitDateSpec.RuleDescription);
            
        var habits = SmokingHabits.Create(cigarettesPerDay, cigarettesPerPack, pricePerPack);
        return new QuitJourney(userId, quitDate, habits);
    }
    
    public void Update(DateTime quitDate, int cigarettesPerDay, int cigarettesPerPack, decimal pricePerPack)
    {
        var quitDateSpec = QuitJourneySpecs.ValidQuitDate();
        if (!quitDateSpec.IsSatisfiedBy(quitDate))
            throw new DomainException(quitDateSpec.RuleDescription);
            
        QuitDate = quitDate;
        SmokingHabits = SmokingHabits.Create(cigarettesPerDay, cigarettesPerPack, pricePerPack);
        SetUpdated();
        
        AddDomainEvent(new JourneyUpdatedEvent(Id, quitDate));
    }
    
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
    
    public Duration GetTimeSinceQuit(DateTime? asOf = null)
    {
        var now = asOf ?? DateTime.UtcNow;
        var timeSpan = now - QuitDate;
        return Duration.FromMinutes((int)timeSpan.TotalMinutes);
    }
    
    public int GetDaysSmokeFree(DateTime? asOf = null)
    {
        var now = asOf ?? DateTime.UtcNow;
        return (int)(now - QuitDate).TotalDays;
    }
    
    public int GetCigarettesAvoided(DateTime? asOf = null)
    {
        var duration = GetTimeSinceQuit(asOf);
        var days = duration.TotalMinutes / (24.0 * 60.0);
        return (int)(days * SmokingHabits.CigarettesPerDay);
    }
    
    public Money GetMoneySaved(DateTime? asOf = null)
    {
        var cigarettesAvoided = GetCigarettesAvoided(asOf);
        return SmokingHabits.PricePerCigarette.Multiply(cigarettesAvoided);
    }
    
    public Duration GetLifeRegained(DateTime? asOf = null)
    {
        var cigarettesAvoided = GetCigarettesAvoided(asOf);
        return Duration.FromMinutes(cigarettesAvoided * MinutesOfLifePerCigarette);
    }
    
    public Milestone GetCurrentMilestone(DateTime? asOf = null)
    {
        var days = GetDaysSmokeFree(asOf);
        return Milestone.GetMilestoneForDays(days);
    }
    
    public Milestone? GetNextMilestone(DateTime? asOf = null)
    {
        var days = GetDaysSmokeFree(asOf);
        return Milestone.GetNextMilestone(days);
    }
    
    public double GetProgressPercentage(DateTime? asOf = null)
    {
        var days = GetDaysSmokeFree(asOf);
        return Math.Min(100, (double)days / 365 * 100);
    }
}
