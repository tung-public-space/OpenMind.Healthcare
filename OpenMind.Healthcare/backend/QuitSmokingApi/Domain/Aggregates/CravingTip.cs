using DDD.BuildingBlocks;

namespace QuitSmokingApi.Domain.Aggregates;

/// <summary>
/// Aggregate root representing a craving tip
/// </summary>
public class CravingTip : AggregateRoot
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Icon { get; private set; } = string.Empty;
    public TipCategory Category { get; private set; }
    
    // Private constructor for EF Core
    private CravingTip() { }
    
    private CravingTip(string title, string description, string icon, TipCategory category)
    {
        Title = title;
        Description = description;
        Icon = icon;
        Category = category;
    }
    
    public static CravingTip Create(string title, string description, string icon, TipCategory category)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Tip title is required");
            
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Tip description is required");
            
        return new CravingTip(title, description, icon, category);
    }
}

public enum TipCategory
{
    Relaxation,
    Physical,
    Exercise,
    Substitute,
    Social,
    Mental
}
