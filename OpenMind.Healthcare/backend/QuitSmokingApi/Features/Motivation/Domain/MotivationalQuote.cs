namespace QuitSmokingApi.Features.Motivation.Domain;

public class MotivationalQuote
{
    public int Id { get; set; }
    public string Quote { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}

public class CravingTip
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}

public class DailyEncouragement
{
    public string Message { get; set; } = string.Empty;
    public MotivationalQuote Quote { get; set; } = null!;
    public List<CravingTip> Tips { get; set; } = new();
    public string SpecialMessage { get; set; } = string.Empty;
}
