using MediatR;
using QuitSmokingApi.Features.Achievements.Domain;
using QuitSmokingApi.Infrastructure.Data;

namespace QuitSmokingApi.Features.Achievements.GetAllAchievements;

public class GetAllAchievementsHandler : IRequestHandler<GetAllAchievementsQuery, List<Achievement>>
{
    private readonly AppDbContext _context;

    public GetAllAchievementsHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Achievement>> Handle(GetAllAchievementsQuery request, CancellationToken cancellationToken)
    {
        var achievements = _context.Achievements.OrderBy(a => a.RequiredDays).ToList();
        var progress = _context.UserProgress.FirstOrDefault();

        if (progress != null)
        {
            var daysSinceQuit = (int)(DateTime.UtcNow - progress.QuitDate).TotalDays;
            foreach (var achievement in achievements)
            {
                achievement.IsUnlocked = daysSinceQuit >= achievement.RequiredDays;
                if (achievement.IsUnlocked)
                {
                    achievement.UnlockedAt = progress.QuitDate.AddDays(achievement.RequiredDays);
                }
            }
        }

        return Task.FromResult(achievements);
    }
}
