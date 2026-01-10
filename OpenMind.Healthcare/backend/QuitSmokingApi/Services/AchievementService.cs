using QuitSmokingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace QuitSmokingApi.Services;

public interface IAchievementService
{
    Task<List<Achievement>> GetAllAchievementsAsync();
    Task<List<Achievement>> GetUnlockedAchievementsAsync();
    Task<Achievement?> CheckForNewAchievementAsync();
}

public class AchievementService : IAchievementService
{
    private readonly Data.AppDbContext _context;

    public AchievementService(Data.AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Achievement>> GetAllAchievementsAsync()
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

        return achievements;
    }

    public async Task<List<Achievement>> GetUnlockedAchievementsAsync()
    {
        var all = await GetAllAchievementsAsync();
        return all.Where(a => a.IsUnlocked).ToList();
    }

    public async Task<Achievement?> CheckForNewAchievementAsync()
    {
        var progress = _context.UserProgress.FirstOrDefault();
        if (progress == null) return null;

        var daysSinceQuit = (int)(DateTime.UtcNow - progress.QuitDate).TotalDays;
        var achievements = _context.Achievements
            .Where(a => a.RequiredDays == daysSinceQuit)
            .FirstOrDefault();

        return achievements;
    }
}
