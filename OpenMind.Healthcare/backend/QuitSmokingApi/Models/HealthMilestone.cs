namespace QuitSmokingApi.Models;

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
