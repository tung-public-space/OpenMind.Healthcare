using QuitSmokingApi.Domain.Common;

namespace QuitSmokingApi.Domain.Aggregates;

/// <summary>
/// Aggregate root representing a motivational quote
/// </summary>
public class MotivationalQuote : AggregateRoot
{
    public string Quote { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public QuoteCategory Category { get; private set; }
    
    // Private constructor for EF Core
    private MotivationalQuote() { }
    
    private MotivationalQuote(string quote, string author, QuoteCategory category)
    {
        Quote = quote;
        Author = author;
        Category = category;
    }
    
    public static MotivationalQuote Create(string quote, string author, QuoteCategory category)
    {
        if (string.IsNullOrWhiteSpace(quote))
            throw new DomainException("Quote text is required");
            
        return new MotivationalQuote(quote, author ?? "Unknown", category);
    }
}

public enum QuoteCategory
{
    Encouragement,
    Motivation,
    Strength,
    Belief,
    Action,
    Benefits,
    Health,
    Discipline,
    Beginning,
    FreshStart,
    Future,
    Cravings,
    Perspective,
    Growth
}
