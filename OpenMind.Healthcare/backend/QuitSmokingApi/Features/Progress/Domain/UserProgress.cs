namespace QuitSmokingApi.Features.Progress.Domain;

public class UserProgress
{
    public int Id { get; set; }
    public DateTime QuitDate { get; set; }
    public int CigarettesPerDay { get; set; }
    public decimal PricePerPack { get; set; }
    public int CigarettesPerPack { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class ProgressStats
{
    public int DaysSmokeFree { get; set; }
    public int HoursSmokeFree { get; set; }
    public int MinutesSmokeFree { get; set; }
    public int CigarettesNotSmoked { get; set; }
    public decimal MoneySaved { get; set; }
    public int LifeRegainedMinutes { get; set; }
    public string LifeRegainedFormatted { get; set; } = string.Empty;
    public double ProgressPercentage { get; set; }
    public string CurrentMilestone { get; set; } = string.Empty;
    public string NextMilestone { get; set; } = string.Empty;
    public int DaysToNextMilestone { get; set; }
}

public class HealthMilestone
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TimeInMinutes { get; set; }
    public string TimeDisplay { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsAchieved { get; set; }
    public double ProgressPercentage { get; set; }
}
