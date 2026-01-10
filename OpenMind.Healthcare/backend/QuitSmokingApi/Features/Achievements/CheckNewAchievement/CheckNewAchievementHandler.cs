using MediatR;
using QuitSmokingApi.Infrastructure.Data;

namespace QuitSmokingApi.Features.Achievements.CheckNewAchievement;

/// <summary>
/// Handler that uses domain model to check for new achievements
/// The achievement unlock logic is in the domain entity
/// </summary>
public class CheckNewAchievementHandler : IRequestHandler<CheckNewAchievementQuery, AchievementDto?>
{
    private readonly AppDbContext _context;

    public CheckNewAchievementHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<AchievementDto?> Handle(CheckNewAchievementQuery request, CancellationToken cancellationToken)
    {
        var journey = _context.QuitJourneys.FirstOrDefault();
        if (journey == null) return Task.FromResult<AchievementDto?>(null);

        var daysSinceQuit = journey.GetDaysSmokeFree();
        
        // Find achievement that is exactly unlocked today
        var achievement = _context.Achievements
            .Where(a => a.IsExactlyUnlockedFor(daysSinceQuit))
            .FirstOrDefault();

        if (achievement == null) return Task.FromResult<AchievementDto?>(null);
        
        return Task.FromResult<AchievementDto?>(new AchievementDto(
            Id: achievement.Id,
            Name: achievement.Name,
            Description: achievement.Description,
            Icon: achievement.Icon,
            RequiredDays: achievement.RequiredDays,
            Category: achievement.Category.ToString(),
            IsUnlocked: true,
            UnlockedAt: journey.QuitDate.AddDays(achievement.RequiredDays),
            ProgressPercentage: 100
        ));
    }
}
