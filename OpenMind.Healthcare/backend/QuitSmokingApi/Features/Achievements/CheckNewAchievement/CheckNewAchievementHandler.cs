using MediatR;
using QuitSmokingApi.Domain.Repositories;
using QuitSmokingApi.Domain.Services;
using QuitSmokingApi.Services;

namespace QuitSmokingApi.Features.Achievements.CheckNewAchievement;

/// <summary>
/// Handler that checks for achievements that are exactly unlocked today.
/// Uses AchievementStatusService to compute status - no need to persist since
/// achievement status is derived from journey's daysSmokeFree.
/// </summary>
public class CheckNewAchievementHandler : IRequestHandler<CheckNewAchievementQuery, AchievementDto?>
{
    private readonly IAchievementRepository _achievementRepository;
    private readonly IQuitJourneyRepository _journeyRepository;
    private readonly AchievementStatusService _achievementStatusService;
    private readonly UserService _userService;

    public CheckNewAchievementHandler(
        IAchievementRepository achievementRepository,
        IQuitJourneyRepository journeyRepository,
        AchievementStatusService achievementStatusService,
        UserService userService)
    {
        _achievementRepository = achievementRepository;
        _journeyRepository = journeyRepository;
        _achievementStatusService = achievementStatusService;
        _userService = userService;
    }

    public async Task<AchievementDto?> Handle(CheckNewAchievementQuery request, CancellationToken cancellationToken)
    {
        var userId = _userService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException("User not authenticated");
        
        var journey = await _journeyRepository.GetByUserIdAsync(userId, cancellationToken);
        if (journey == null) return null;

        var achievements = await _achievementRepository.GetAllAsync(cancellationToken);
        
        // Find achievement that is exactly unlocked today (daysSmokeFree == requiredDays)
        var daysSmokeFree = journey.GetDaysSmokeFree();
        var achievement = achievements.FirstOrDefault(a => a.IsExactlyUnlockedFor(daysSmokeFree));

        if (achievement == null) return null;
        
        // Compute status using domain service
        var status = _achievementStatusService.ComputeStatus(achievement, journey);
        
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
