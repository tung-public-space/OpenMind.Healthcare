namespace DDD.BuildingBlocks;

/// <summary>
/// Base class for aggregate roots - entities that are the entry point for a cluster of domain objects.
/// Aggregates are consistency boundaries that enforce business rules (invariants).
/// </summary>
public abstract class AggregateRoot : Entity
{
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;
    
    protected void SetUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Checks a business rule and throws a DomainException if the rule is broken.
    /// Use this method to enforce invariants before state changes.
    /// </summary>
    /// <param name="rule">The business rule to check</param>
    /// <exception cref="DomainException">Thrown when the rule is broken</exception>
    protected static void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
        {
            throw new BusinessRuleValidationException(rule);
        }
    }
    
    /// <summary>
    /// Checks multiple business rules and throws if any rule is broken.
    /// </summary>
    /// <param name="rules">The business rules to check</param>
    protected static void CheckRules(params IBusinessRule[] rules)
    {
        foreach (var rule in rules)
        {
            CheckRule(rule);
        }
    }
}

/// <summary>
/// Exception thrown when a business rule validation fails
/// </summary>
public class BusinessRuleValidationException : DomainException
{
    public IBusinessRule BrokenRule { get; }
    public string RuleName => BrokenRule.RuleName;
    
    public BusinessRuleValidationException(IBusinessRule brokenRule)
        : base(brokenRule.ErrorMessage)
    {
        BrokenRule = brokenRule;
    }
}
