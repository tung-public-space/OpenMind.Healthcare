using Microsoft.EntityFrameworkCore;
using QuitSmokingApi.Features.Achievements.Domain;
using QuitSmokingApi.Features.Motivation.Domain;
using QuitSmokingApi.Features.Progress.Domain;

namespace QuitSmokingApi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserProgress> UserProgress { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<UserAchievement> UserAchievements { get; set; }
    public DbSet<MotivationalQuote> MotivationalQuotes { get; set; }
    public DbSet<CravingTip> CravingTips { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
