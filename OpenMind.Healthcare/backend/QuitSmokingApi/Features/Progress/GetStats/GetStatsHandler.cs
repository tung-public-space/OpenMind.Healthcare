using MediatR;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Domain.ValueObjects;
using QuitSmokingApi.Infrastructure.Data;

namespace QuitSmokingApi.Features.Progress.GetStats;

/// <summary>
/// Handler that leverages the rich domain model to calculate statistics
/// The business logic is now encapsulated in the QuitJourney aggregate
/// </summary>
public class GetStatsHandler : IRequestHandler<GetStatsQuery, ProgressStatistics>
{
    private readonly AppDbContext _context;

    public GetStatsHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<ProgressStatistics> Handle(GetStatsQuery request, CancellationToken cancellationToken)
    {
        var journey = _context.QuitJourneys.FirstOrDefault();
        
        if (journey == null)
        {
            // Return empty statistics using domain defaults
            return Task.FromResult(new ProgressStatistics(
                daysSmokeFree: 0,
                hoursSmokeFree: 0,
                minutesSmokeFree: 0,
                cigarettesAvoided: 0,
                moneySaved: Money.Zero(),
                lifeRegained: Duration.Zero,
                progressPercentage: 0,
                currentMilestone: Milestone.FirstStep,
                nextMilestone: Milestone.TwentyFourHours,
                daysToNextMilestone: 1
            ));
        }

        // All business logic is now in the domain - just delegate to the aggregate
        var statistics = journey.GetStatistics();
        return Task.FromResult(statistics);
    }
}
