using MediatR;
using QuitSmokingApi.Domain.Repositories;
using QuitSmokingApi.Domain.Services;
using QuitSmokingApi.Services;

namespace QuitSmokingApi.Features.Achievements.GetAllAchievements;

/// <summary>
/// Handler that uses domain services and repositories following DDD principles.
/// The business logic is encapsulated in the domain service.
/// </summary>
public class GetAllAchievementsHandler : IRequestHandler<GetAllAchievementsQuery, List<AchievementDto>>
{
    private readonly IAchievementRepository _achievementRepository;
    private readonly IQuitJourneyRepository _journeyRepository;
    private readonly AchievementStatusService _achievementStatusService;
    private readonly UserService _userService;

    public GetAllAchievementsHandler(
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

    public async Task<List<AchievementDto>> Handle(GetAllAchievementsQuery request, CancellationToken cancellationToken)
    {
        var userId = _userService.GetCurrentUserId() 
            ?? throw new UnauthorizedAccessException("User not authenticated");
        
        // Use repositories to fetch aggregates
        var achievements = await _achievementRepository.GetAllOrderedByRequiredDaysAsync(cancellationToken);
        var journey = await _journeyRepository.GetByUserIdAsync(userId, cancellationToken);

        // Delegate business logic to domain service
        var statuses = _achievementStatusService.ComputeStatuses(achievements, journey);

        // Map to DTOs
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
