namespace QuitSmokingApi.Domain.Common;

/// <summary>
/// Base class for aggregate roots - entities that are the entry point for a cluster of domain objects
/// </summary>
public abstract class AggregateRoot : Entity
{
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;
    
    protected void SetUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
