using MediatR;
using QuitSmokingApi.Features.Progress.Domain;
using QuitSmokingApi.Infrastructure.Data;

namespace QuitSmokingApi.Features.Progress.GetStats;

public class GetStatsHandler : IRequestHandler<GetStatsQuery, ProgressStats>
{
    private readonly AppDbContext _context;

    public GetStatsHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<ProgressStats> Handle(GetStatsQuery request, CancellationToken cancellationToken)
    {
        var progress = _context.UserProgress.FirstOrDefault();
        if (progress == null)
        {
            return Task.FromResult(new ProgressStats());
        }

        var timeSinceQuit = DateTime.UtcNow - progress.QuitDate;
        var totalMinutes = (int)timeSinceQuit.TotalMinutes;
        var cigarettesNotSmoked = (int)(timeSinceQuit.TotalDays * progress.CigarettesPerDay);
        var pricePerCigarette = progress.PricePerPack / progress.CigarettesPerPack;
        var moneySaved = cigarettesNotSmoked * pricePerCigarette;
        
        // Each cigarette takes approximately 11 minutes off your life
        var lifeRegainedMinutes = cigarettesNotSmoked * 11;
        var days = (int)timeSinceQuit.TotalDays;

        var stats = new ProgressStats
        {
            DaysSmokeFree = days,
            HoursSmokeFree = (int)timeSinceQuit.TotalHours,
            MinutesSmokeFree = totalMinutes,
            CigarettesNotSmoked = cigarettesNotSmoked,
            MoneySaved = Math.Round(moneySaved, 2),
            LifeRegainedMinutes = lifeRegainedMinutes,
            LifeRegainedFormatted = FormatLifeRegained(lifeRegainedMinutes),
            ProgressPercentage = CalculateProgressPercentage(days),
            CurrentMilestone = GetCurrentMilestone(days),
            NextMilestone = GetNextMilestone(days),
            DaysToNextMilestone = GetDaysToNextMilestone(days)
        };

        return Task.FromResult(stats);
    }

    private static string FormatLifeRegained(int minutes)
    {
        if (minutes < 60) return $"{minutes} minutes";
        if (minutes < 1440) return $"{minutes / 60} hours {minutes % 60} minutes";
        var days = minutes / 1440;
        var hours = (minutes % 1440) / 60;
        return $"{days} days {hours} hours";
    }

    private static double CalculateProgressPercentage(int days)
    {
        // Progress towards 1 year milestone
        return Math.Min(100, (double)days / 365 * 100);
    }

    private static string GetCurrentMilestone(int days)
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

    private static string GetNextMilestone(int days)
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

    private static int GetDaysToNextMilestone(int days)
    {
        int[] milestones = { 1, 3, 7, 14, 21, 30, 42, 60, 90, 180, 365 };
        foreach (var m in milestones)
        {
            if (days < m) return m - days;
        }
        return 0;
    }
}
