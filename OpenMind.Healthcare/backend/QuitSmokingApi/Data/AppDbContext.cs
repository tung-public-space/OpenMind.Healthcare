using Microsoft.EntityFrameworkCore;
using QuitSmokingApi.Models;

namespace QuitSmokingApi.Data;

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
