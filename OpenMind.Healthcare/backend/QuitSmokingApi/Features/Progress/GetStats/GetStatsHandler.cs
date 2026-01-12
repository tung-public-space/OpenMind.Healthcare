using MediatR;
using QuitSmokingApi.Domain.Repositories;
using QuitSmokingApi.Domain.ValueObjects;
using QuitSmokingApi.Services;

namespace QuitSmokingApi.Features.Progress.GetStats;

public record GetStatsQuery : IRequest<ProgressStatistics>;

public class GetStatsHandler(
    IQuitJourneyRepository journeyRepository,
    UserService userService) : IRequestHandler<GetStatsQuery, ProgressStatistics>
{
    public async Task<ProgressStatistics> Handle(GetStatsQuery request, CancellationToken cancellationToken)
    {
        var userId = userService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException("User not authenticated");
        
        var journey = await journeyRepository.GetByUserIdAsync(userId, cancellationToken);
        
        if (journey == null)
        {
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

        var statistics = journey.GetStatistics();
        return statistics;
    }
}
