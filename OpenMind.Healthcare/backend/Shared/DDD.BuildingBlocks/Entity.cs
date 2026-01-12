namespace DDD.BuildingBlocks;

/// <summary>
/// Base class for all domain entities
/// </summary>
public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();
    
    public Guid Id { get; protected set; } = Guid.NewGuid();
    
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    protected void Emit(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
            return false;
            
        if (ReferenceEquals(this, other))
            return true;
            
        if (GetType() != other.GetType())
            return false;
            
        if (Id == Guid.Empty || other.Id == Guid.Empty)
            return false;
            
        return Id == other.Id;
    }
    
    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }
    
    public static bool operator ==(Entity? a, Entity? b)
    {
        if (a is null && b is null)
            return true;
            
        if (a is null || b is null)
            return false;
            
        return a.Equals(b);
    }
    
    public static bool operator !=(Entity? a, Entity? b)
    {
        return !(a == b);
    }
}
