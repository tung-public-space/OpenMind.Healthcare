using QuitSmokingApi.Domain.Common;

namespace QuitSmokingApi.Domain.ValueObjects;

/// <summary>
/// Value object representing a time duration with rich formatting capabilities
/// </summary>
public class Duration : ValueObject
{
    public int TotalMinutes { get; }
    
    private Duration(int totalMinutes)
    {
        TotalMinutes = totalMinutes;
    }
    
    public static Duration FromMinutes(int minutes)
    {
        if (minutes < 0)
            throw new DomainException("Duration cannot be negative");
            
        return new Duration(minutes);
    }
    
    public static Duration FromHours(int hours) => FromMinutes(hours * 60);
    
    public static Duration FromDays(int days) => FromMinutes(days * 24 * 60);
    
    public static Duration FromTimeSpan(TimeSpan timeSpan) => FromMinutes((int)timeSpan.TotalMinutes);
    
    public static Duration Zero => new(0);
    
    public int Days => TotalMinutes / (24 * 60);
    public int Hours => TotalMinutes / 60;
    public int Minutes => TotalMinutes % 60;
    public int HoursWithinDay => (TotalMinutes % (24 * 60)) / 60;
    
    public string ToFriendlyString()
    {
        if (TotalMinutes < 60)
            return $"{TotalMinutes} minutes";
            
        if (TotalMinutes < 24 * 60)
        {
            var hours = TotalMinutes / 60;
            var mins = TotalMinutes % 60;
            return mins > 0 ? $"{hours} hours {mins} minutes" : $"{hours} hours";
        }
        
        var days = Days;
        var remainingHours = HoursWithinDay;
        return remainingHours > 0 ? $"{days} days {remainingHours} hours" : $"{days} days";
    }
    
    public string ToCompactString()
    {
        if (TotalMinutes < 60) return $"{TotalMinutes}m";
        if (TotalMinutes < 24 * 60) return $"{Hours}h";
        if (TotalMinutes < 7 * 24 * 60) return $"{Days}d";
        if (TotalMinutes < 30 * 24 * 60) return $"{Days / 7}w";
        if (TotalMinutes < 365 * 24 * 60) return $"{Days / 30}mo";
        return $"{Days / 365}y";
    }
    
    public Duration Add(Duration other) => new(TotalMinutes + other.TotalMinutes);
    
    public bool IsGreaterThan(Duration other) => TotalMinutes > other.TotalMinutes;
    
    public bool IsGreaterThanOrEqual(Duration other) => TotalMinutes >= other.TotalMinutes;
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return TotalMinutes;
    }
}
