using MediatR;
using QuitSmokingApi.Domain.Repositories;
using QuitSmokingApi.Domain.Services;
using QuitSmokingApi.Services;

namespace QuitSmokingApi.Features.Achievements.GetAllAchievements;

public record GetAllAchievementsQuery : IRequest<List<AchievementDto>>;

public class GetAllAchievementsHandler(
    IAchievementRepository achievementRepository,
    IQuitJourneyRepository journeyRepository,
    AchievementStatusService achievementStatusService,
    UserService userService) : IRequestHandler<GetAllAchievementsQuery, List<AchievementDto>>
{
    public async Task<List<AchievementDto>> Handle(GetAllAchievementsQuery request, CancellationToken cancellationToken)
    {
        var userId = userService.GetCurrentUserId() 
            ?? throw new UnauthorizedAccessException("User not authenticated");
        
        var achievements = await achievementRepository.GetAllOrderedByRequiredDaysAsync(cancellationToken);
        var journey = await journeyRepository.GetByUserIdAsync(userId, cancellationToken);

        var statuses = achievementStatusService.ComputeStatuses(achievements, journey);

        var dtos = statuses.Select(s => new AchievementDto(
            Id: s.AchievementId,
            Name: s.Name,
            Description: s.Description,
            Icon: s.Icon,
            RequiredDays: s.RequiredDays,
            Category: s.Category,
            IsUnlocked: s.IsUnlocked,
            UnlockedAt: s.UnlockedAt,
            ProgressPercentage: s.ProgressPercentage
        )).ToList();

        return dtos;
    }
}
