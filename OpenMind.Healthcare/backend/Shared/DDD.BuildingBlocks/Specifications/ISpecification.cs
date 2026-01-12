using System.Linq.Expressions;

namespace DDD.BuildingBlocks.Specifications;

/// <summary>
/// Base specification interface for encapsulating business rules
/// </summary>
/// <typeparam name="T">The type of entity the specification applies to</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Checks if the entity satisfies the specification
    /// </summary>
    bool IsSatisfiedBy(T entity);
    
    /// <summary>
    /// Returns an expression for use with LINQ queries
    /// </summary>
    Expression<Func<T, bool>> ToExpression();
    
    /// <summary>
    /// Returns the business rule description
    /// </summary>
    string RuleDescription { get; }
}
