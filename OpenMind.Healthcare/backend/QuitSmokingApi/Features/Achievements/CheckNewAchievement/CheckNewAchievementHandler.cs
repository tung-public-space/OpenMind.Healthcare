using MediatR;
using QuitSmokingApi.Features.Achievements.Domain;
using QuitSmokingApi.Infrastructure.Data;

namespace QuitSmokingApi.Features.Achievements.CheckNewAchievement;

public class CheckNewAchievementHandler : IRequestHandler<CheckNewAchievementQuery, Achievement?>
{
    private readonly AppDbContext _context;

    public CheckNewAchievementHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<Achievement?> Handle(CheckNewAchievementQuery request, CancellationToken cancellationToken)
    {
        var progress = _context.UserProgress.FirstOrDefault();
        if (progress == null) return Task.FromResult<Achievement?>(null);

        var daysSinceQuit = (int)(DateTime.UtcNow - progress.QuitDate).TotalDays;
        var achievement = _context.Achievements
            .Where(a => a.RequiredDays == daysSinceQuit)
            .FirstOrDefault();

        return Task.FromResult(achievement);
    }
}
