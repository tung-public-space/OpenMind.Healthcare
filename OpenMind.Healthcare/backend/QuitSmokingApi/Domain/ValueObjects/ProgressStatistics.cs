using DDD.BuildingBlocks;

namespace QuitSmokingApi.Domain.ValueObjects;

/// <summary>
/// Value object representing progress statistics - read-only snapshot of journey progress.
/// Value objects are immutable and identified by their values, not by an ID.
/// </summary>
public class ProgressStatistics(
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
    : ValueObject
{
    public int DaysSmokeFree { get; } = daysSmokeFree;
    public int HoursSmokeFree { get; } = hoursSmokeFree;
    public int MinutesSmokeFree { get; } = minutesSmokeFree;
    public int CigarettesAvoided { get; } = cigarettesAvoided;
    public Money MoneySaved { get; } = moneySaved;
    public Duration LifeRegained { get; } = lifeRegained;
    public double ProgressPercentage { get; } = Math.Round(progressPercentage, 2);
    public Milestone CurrentMilestone { get; } = currentMilestone;
    public Milestone? NextMilestone { get; } = nextMilestone;
    public int DaysToNextMilestone { get; } = daysToNextMilestone;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return DaysSmokeFree;
        yield return CigarettesAvoided;
        yield return MoneySaved;
    }
}
