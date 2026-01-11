using MediatR;
using Microsoft.EntityFrameworkCore;
using QuitSmokingApi.Infrastructure.Data;
using QuitSmokingApi.Services;

namespace QuitSmokingApi.Features.Achievements.CheckNewAchievement;

/// <summary>
/// Handler that uses domain model to check for new achievements
/// The achievement unlock logic is in the domain entity
/// </summary>
public class CheckNewAchievementHandler : IRequestHandler<CheckNewAchievementQuery, AchievementDto?>
{
    private readonly AppDbContext _context;
    private readonly UserService _userService;

    public CheckNewAchievementHandler(AppDbContext context, UserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<AchievementDto?> Handle(CheckNewAchievementQuery request, CancellationToken cancellationToken)
    {
        var userId = _userService.GetCurrentUserId();
        
        var journey = await _context.QuitJourneys.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        if (journey == null) return null;

        var daysSinceQuit = journey.GetDaysSmokeFree();
        
        // Find achievement that is exactly unlocked today
        var achievement = await _context.Achievements
            .Where(a => a.IsExactlyUnlockedFor(daysSinceQuit))
            .FirstOrDefaultAsync(cancellationToken);

        if (achievement == null) return null;
        
        return new AchievementDto(
            Id: achievement.Id,
            Name: achievement.Name,
            Description: achievement.Description,
            Icon: achievement.Icon,
            RequiredDays: achievement.RequiredDays,
            Category: achievement.Category.ToString(),
            IsUnlocked: true,
            UnlockedAt: journey.QuitDate.AddDays(achievement.RequiredDays),
            ProgressPercentage: 100
        );
    }
}
