namespace DDD.BuildingBlocks;

/// <summary>
/// Interface for business rules that can be checked on aggregates.
/// Business rules encapsulate domain invariants that must hold true.
/// </summary>
public interface IBusinessRule
{
    /// <summary>
    /// The name of the business rule for identification
    /// </summary>
    string RuleName { get; }
    
    /// <summary>
    /// The error message to display when the rule is broken
    /// </summary>
    string ErrorMessage { get; }
    
    /// <summary>
    /// Checks if the business rule is violated
    /// </summary>
    /// <returns>True if the rule is broken (invariant violated), false otherwise</returns>
    bool IsBroken();
}
