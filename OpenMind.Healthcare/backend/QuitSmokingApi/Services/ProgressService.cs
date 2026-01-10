using QuitSmokingApi.Models;

namespace QuitSmokingApi.Services;

public interface IProgressService
{
    Task<UserProgress?> GetUserProgressAsync();
    Task<UserProgress> CreateOrUpdateProgressAsync(UserProgress progress);
    Task<ProgressStats> GetProgressStatsAsync();
    Task<List<HealthMilestone>> GetHealthMilestonesAsync();
}

public class ProgressService : IProgressService
{
    private readonly Data.AppDbContext _context;

    public ProgressService(Data.AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserProgress?> GetUserProgressAsync()
    {
        return _context.UserProgress.FirstOrDefault();
    }

    public async Task<UserProgress> CreateOrUpdateProgressAsync(UserProgress progress)
    {
        var existing = _context.UserProgress.FirstOrDefault();
        if (existing != null)
        {
            existing.QuitDate = progress.QuitDate;
            existing.CigarettesPerDay = progress.CigarettesPerDay;
            existing.PricePerPack = progress.PricePerPack;
            existing.CigarettesPerPack = progress.CigarettesPerPack;
            existing.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();
            return existing;
        }

        progress.CreatedAt = DateTime.UtcNow;
        progress.UpdatedAt = DateTime.UtcNow;
        _context.UserProgress.Add(progress);
        _context.SaveChanges();
        return progress;
    }

    public async Task<ProgressStats> GetProgressStatsAsync()
    {
        var progress = _context.UserProgress.FirstOrDefault();
        if (progress == null)
        {
            return new ProgressStats();
        }

        var timeSinceQuit = DateTime.UtcNow - progress.QuitDate;
        var totalMinutes = (int)timeSinceQuit.TotalMinutes;
        var cigarettesNotSmoked = (int)(timeSinceQuit.TotalDays * progress.CigarettesPerDay);
        var pricePerCigarette = progress.PricePerPack / progress.CigarettesPerPack;
        var moneySaved = cigarettesNotSmoked * pricePerCigarette;
        
        // Each cigarette takes approximately 11 minutes off your life
        var lifeRegainedMinutes = cigarettesNotSmoked * 11;

        var stats = new ProgressStats
        {
            DaysSmokeFree = (int)timeSinceQuit.TotalDays,
            HoursSmokeFree = (int)timeSinceQuit.TotalHours,
            MinutesSmokeFree = totalMinutes,
            CigarettesNotSmoked = cigarettesNotSmoked,
            MoneySaved = Math.Round(moneySaved, 2),
            LifeRegainedMinutes = lifeRegainedMinutes,
            LifeRegainedFormatted = FormatLifeRegained(lifeRegainedMinutes),
            ProgressPercentage = CalculateProgressPercentage((int)timeSinceQuit.TotalDays),
            CurrentMilestone = GetCurrentMilestone((int)timeSinceQuit.TotalDays),
            NextMilestone = GetNextMilestone((int)timeSinceQuit.TotalDays),
            DaysToNextMilestone = GetDaysToNextMilestone((int)timeSinceQuit.TotalDays)
        };

        return stats;
    }

    public async Task<List<HealthMilestone>> GetHealthMilestonesAsync()
    {
        var progress = _context.UserProgress.FirstOrDefault();
        var minutesSinceQuit = progress != null 
            ? (int)(DateTime.UtcNow - progress.QuitDate).TotalMinutes 
            : 0;

        var milestones = new List<HealthMilestone>
        {
            new() { Id = 1, Title = "Blood Pressure Normalizes", Description = "Your blood pressure and pulse rate begin to return to normal.", TimeInMinutes = 20, TimeDisplay = "20 minutes", Icon = "❤️", Category = "Cardiovascular" },
            new() { Id = 2, Title = "Carbon Monoxide Levels Drop", Description = "Carbon monoxide level in your blood drops to normal.", TimeInMinutes = 480, TimeDisplay = "8 hours", Icon = "🫁", Category = "Respiratory" },
            new() { Id = 3, Title = "Heart Attack Risk Decreases", Description = "Your risk of heart attack begins to decrease.", TimeInMinutes = 1440, TimeDisplay = "24 hours", Icon = "💗", Category = "Cardiovascular" },
            new() { Id = 4, Title = "Nerve Endings Regenerate", Description = "Nerve endings start to regenerate. Taste and smell improve.", TimeInMinutes = 2880, TimeDisplay = "48 hours", Icon = "👃", Category = "Sensory" },
            new() { Id = 5, Title = "Breathing Improves", Description = "Bronchial tubes relax, making breathing easier. Lung capacity increases.", TimeInMinutes = 4320, TimeDisplay = "72 hours", Icon = "🌬️", Category = "Respiratory" },
            new() { Id = 6, Title = "Circulation Improves", Description = "Blood circulation improves significantly. Walking becomes easier.", TimeInMinutes = 14400, TimeDisplay = "2 weeks", Icon = "🩸", Category = "Cardiovascular" },
            new() { Id = 7, Title = "Lung Function Increases", Description = "Lung function increases up to 30%. Coughing and shortness of breath decrease.", TimeInMinutes = 43200, TimeDisplay = "1 month", Icon = "💨", Category = "Respiratory" },
            new() { Id = 8, Title = "Cilia Regenerate", Description = "Cilia in lungs regenerate, improving ability to clean lungs and reduce infection.", TimeInMinutes = 129600, TimeDisplay = "3 months", Icon = "🧹", Category = "Respiratory" },
            new() { Id = 9, Title = "Energy Levels Increase", Description = "Overall energy increases. Physical activity becomes much easier.", TimeInMinutes = 259200, TimeDisplay = "6 months", Icon = "⚡", Category = "Energy" },
            new() { Id = 10, Title = "Coronary Heart Disease Risk Halved", Description = "Your risk of coronary heart disease is now half that of a smoker.", TimeInMinutes = 525600, TimeDisplay = "1 year", Icon = "🫀", Category = "Cardiovascular" },
            new() { Id = 11, Title = "Stroke Risk Reduced", Description = "Your stroke risk is reduced to that of a non-smoker.", TimeInMinutes = 2628000, TimeDisplay = "5 years", Icon = "🧠", Category = "Cardiovascular" },
            new() { Id = 12, Title = "Lung Cancer Risk Halved", Description = "Risk of lung cancer drops to half that of a smoker.", TimeInMinutes = 5256000, TimeDisplay = "10 years", Icon = "🎗️", Category = "Cancer Prevention" },
            new() { Id = 13, Title = "Heart Disease Risk Normal", Description = "Risk of coronary heart disease same as non-smoker.", TimeInMinutes = 7884000, TimeDisplay = "15 years", Icon = "💖", Category = "Cardiovascular" }
        };

        foreach (var milestone in milestones)
        {
            milestone.IsAchieved = minutesSinceQuit >= milestone.TimeInMinutes;
            milestone.ProgressPercentage = Math.Min(100, (double)minutesSinceQuit / milestone.TimeInMinutes * 100);
        }

        return milestones;
    }

    private string FormatLifeRegained(int minutes)
    {
        if (minutes < 60) return $"{minutes} minutes";
        if (minutes < 1440) return $"{minutes / 60} hours {minutes % 60} minutes";
        var days = minutes / 1440;
        var hours = (minutes % 1440) / 60;
        return $"{days} days {hours} hours";
    }

    private double CalculateProgressPercentage(int days)
    {
        // Progress towards 1 year milestone
        return Math.Min(100, (double)days / 365 * 100);
    }

    private string GetCurrentMilestone(int days)
    {
        if (days >= 365) return "Year One Legend";
        if (days >= 180) return "Half Year Hero";
        if (days >= 90) return "Quarter Year Champion";
        if (days >= 60) return "Two Month Titan";
        if (days >= 42) return "Six Week Superstar";
        if (days >= 30) return "Monthly Master";
        if (days >= 21) return "Three Week Hero";
        if (days >= 14) return "Two Week Champion";
        if (days >= 7) return "One Week Wonder";
        if (days >= 3) return "Weekend Warrior";
        if (days >= 1) return "24 Hours Strong";
        return "First Step";
    }

    private string GetNextMilestone(int days)
    {
        if (days >= 365) return "You've completed all major milestones!";
        if (days >= 180) return "Year One Legend";
        if (days >= 90) return "Half Year Hero";
        if (days >= 60) return "Quarter Year Champion";
        if (days >= 42) return "Two Month Titan";
        if (days >= 30) return "Six Week Superstar";
        if (days >= 21) return "Monthly Master";
        if (days >= 14) return "Three Week Hero";
        if (days >= 7) return "Two Week Champion";
        if (days >= 3) return "One Week Wonder";
        if (days >= 1) return "Weekend Warrior";
        return "24 Hours Strong";
    }

    private int GetDaysToNextMilestone(int days)
    {
        int[] milestones = { 1, 3, 7, 14, 21, 30, 42, 60, 90, 180, 365 };
        foreach (var m in milestones)
        {
            if (days < m) return m - days;
        }
        return 0;
    }
}
