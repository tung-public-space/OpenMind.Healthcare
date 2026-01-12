using MediatR;
using QuitSmokingApi.Domain.Repositories;
using QuitSmokingApi.Domain.ValueObjects;
using QuitSmokingApi.Services;

namespace QuitSmokingApi.Features.Progress.GetStats;

/// <summary>
/// Handler that leverages the rich domain model to calculate statistics.
/// Uses repository for data access following DDD principles.
/// </summary>
public class GetStatsHandler : IRequestHandler<GetStatsQuery, ProgressStatistics>
{
    private readonly IQuitJourneyRepository _journeyRepository;
    private readonly UserService _userService;

    public GetStatsHandler(IQuitJourneyRepository journeyRepository, UserService userService)
    {
        _journeyRepository = journeyRepository;
        _userService = userService;
    }

    public async Task<ProgressStatistics> Handle(GetStatsQuery request, CancellationToken cancellationToken)
    {
        var userId = _userService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException("User not authenticated");
        
        var journey = await _journeyRepository.GetByUserIdAsync(userId, cancellationToken);
        
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
