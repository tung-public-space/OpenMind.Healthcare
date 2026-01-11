using MediatR;
using Microsoft.EntityFrameworkCore;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Domain.ValueObjects;
using QuitSmokingApi.Infrastructure.Data;
using QuitSmokingApi.Services;

namespace QuitSmokingApi.Features.Progress.GetStats;

/// <summary>
/// Handler that leverages the rich domain model to calculate statistics
/// The business logic is now encapsulated in the QuitJourney aggregate
/// </summary>
public class GetStatsHandler : IRequestHandler<GetStatsQuery, ProgressStatistics>
{
    private readonly AppDbContext _context;
    private readonly UserService _userService;

    public GetStatsHandler(AppDbContext context, UserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<ProgressStatistics> Handle(GetStatsQuery request, CancellationToken cancellationToken)
    {
        var userId = _userService.GetCurrentUserId();
        
        var journey = await _context.QuitJourneys.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        
        if (journey == null)
        {
            // Return empty statistics using domain defaults
            return new ProgressStatistics(
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
            );
        }

        // All business logic is now in the domain - just delegate to the aggregate
        var statistics = journey.GetStatistics();
        return statistics;
    }
}
