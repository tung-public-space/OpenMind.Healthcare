using MediatR;
using QuitSmokingApi.Domain.Repositories;
using QuitSmokingApi.Domain.Services;
using QuitSmokingApi.Services;

namespace QuitSmokingApi.Features.Achievements.CheckNewAchievement;

public record CheckNewAchievementQuery : IRequest<AchievementDto?>;

public class CheckNewAchievementHandler(
    IAchievementRepository achievementRepository,
    IQuitJourneyRepository journeyRepository,
    AchievementStatusService achievementStatusService,
    UserService userService) : IRequestHandler<CheckNewAchievementQuery, AchievementDto?>
{
    public async Task<AchievementDto?> Handle(CheckNewAchievementQuery request, CancellationToken cancellationToken)
    {
        var userId = userService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException("User not authenticated");
        
        var journey = await journeyRepository.GetByUserIdAsync(userId, cancellationToken);
        if (journey == null) return null;

        var achievements = await achievementRepository.GetAllAsync(cancellationToken);
        
        var daysSmokeFree = journey.GetDaysSmokeFree();
        var achievement = achievements.FirstOrDefault(a => a.IsExactlyUnlockedFor(daysSmokeFree));

        if (achievement == null) return null;
        
        var status = achievementStatusService.ComputeStatus(achievement, journey);
        
        return new AchievementDto(
            Id: status.AchievementId,
            Name: status.Name,
            Description: status.Description,
            Icon: status.Icon,
            RequiredDays: status.RequiredDays,
            Category: status.Category,
            IsUnlocked: status.IsUnlocked,
            UnlockedAt: status.UnlockedAt,
            ProgressPercentage: status.ProgressPercentage
        );
    }
}
