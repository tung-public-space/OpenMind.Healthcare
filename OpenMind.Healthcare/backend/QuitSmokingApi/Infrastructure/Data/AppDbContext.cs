using Microsoft.EntityFrameworkCore;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Domain.ValueObjects;

namespace QuitSmokingApi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<QuitJourney> QuitJourneys { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<UserAchievement> UserAchievements { get; set; }
    public DbSet<MotivationalQuote> MotivationalQuotes { get; set; }
    public DbSet<CravingTip> CravingTips { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure QuitJourney aggregate
        modelBuilder.Entity<QuitJourney>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired();
            entity.HasIndex(e => e.UserId).IsUnique();
            entity.Property(e => e.QuitDate).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            
            // Configure SmokingHabits value object as owned entity
            entity.OwnsOne(e => e.SmokingHabits, habits =>
            {
                habits.Property(h => h.CigarettesPerDay).HasColumnName("CigarettesPerDay");
                habits.Property(h => h.CigarettesPerPack).HasColumnName("CigarettesPerPack");
                
                // Configure Money value object within SmokingHabits
                habits.OwnsOne(h => h.PricePerPack, price =>
                {
                    price.Property(p => p.Amount).HasColumnName("PricePerPack").HasPrecision(18, 2);
                    price.Property(p => p.Currency).HasColumnName("Currency").HasMaxLength(3).HasDefaultValue("USD");
                });
            });
            
            entity.Ignore(e => e.DomainEvents);
        });
        
        // Configure Achievement
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Icon).HasMaxLength(10);
            entity.Property(e => e.RequiredDays).IsRequired();
            entity.Property(e => e.Category).HasConversion<string>().HasMaxLength(50);
            entity.Ignore(e => e.DomainEvents);
        });
        
        // Configure UserAchievement
        modelBuilder.Entity<UserAchievement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.UnlockedAt).IsRequired();
            entity.HasOne(e => e.Achievement).WithMany().HasForeignKey(e => e.AchievementId);
            entity.Ignore(e => e.DomainEvents);
        });
        
        // Configure MotivationalQuote
        modelBuilder.Entity<MotivationalQuote>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quote).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.Category).HasConversion<string>().HasMaxLength(50);
            entity.Ignore(e => e.DomainEvents);
        });
        
        // Configure CravingTip
        modelBuilder.Entity<CravingTip>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Icon).HasMaxLength(10);
            entity.Property(e => e.Category).HasConversion<string>().HasMaxLength(50);
            entity.Ignore(e => e.DomainEvents);
        });
    }
}
