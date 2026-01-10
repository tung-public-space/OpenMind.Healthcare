using QuitSmokingApi.Domain.Common;
using QuitSmokingApi.Domain.ValueObjects;

namespace QuitSmokingApi.Domain.Aggregates;

/// <summary>
/// Value object representing progress statistics - read-only snapshot of journey progress
/// </summary>
public class ProgressStatistics : ValueObject
{
    public int DaysSmokeFree { get; }
    public int HoursSmokeFree { get; }
    public int MinutesSmokeFree { get; }
    public int CigarettesAvoided { get; }
    public Money MoneySaved { get; }
    public Duration LifeRegained { get; }
    public double ProgressPercentage { get; }
    public Milestone CurrentMilestone { get; }
    public Milestone? NextMilestone { get; }
    public int DaysToNextMilestone { get; }
    
    public ProgressStatistics(
        int daysSmokeFree,
        int hoursSmokeFree,
        int minutesSmokeFree,
        int cigarettesAvoided,
        Money moneySaved,
        Duration lifeRegained,
        double progressPercentage,
        Milestone currentMilestone,
        Milestone? nextMilestone,
        int daysToNextMilestone)
    {
        DaysSmokeFree = daysSmokeFree;
        HoursSmokeFree = hoursSmokeFree;
        MinutesSmokeFree = minutesSmokeFree;
        CigarettesAvoided = cigarettesAvoided;
        MoneySaved = moneySaved;
        LifeRegained = lifeRegained;
        ProgressPercentage = Math.Round(progressPercentage, 2);
        CurrentMilestone = currentMilestone;
        NextMilestone = nextMilestone;
        DaysToNextMilestone = daysToNextMilestone;
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return DaysSmokeFree;
        yield return CigarettesAvoided;
        yield return MoneySaved;
    }
}
