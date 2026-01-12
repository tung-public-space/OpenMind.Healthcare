using System.Linq.Expressions;

namespace DDD.BuildingBlocks;

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

/// <summary>
/// Abstract base class for specifications with composite support
/// </summary>
/// <typeparam name="T">The type of entity the specification applies to</typeparam>
public abstract class Specification<T> : ISpecification<T>
{
    public abstract string RuleDescription { get; }
    
    public bool IsSatisfiedBy(T entity)
    {
        var predicate = ToExpression().Compile();
        return predicate(entity);
    }
    
    public abstract Expression<Func<T, bool>> ToExpression();
    
    public static Specification<T> operator &(Specification<T> left, Specification<T> right)
        => new AndSpecification<T>(left, right);
    
    public static Specification<T> operator |(Specification<T> left, Specification<T> right)
        => new OrSpecification<T>(left, right);
    
    public static Specification<T> operator !(Specification<T> specification)
        => new NotSpecification<T>(specification);
}

/// <summary>
/// Combines two specifications with logical AND
/// </summary>
public class AndSpecification<T>(Specification<T> left, Specification<T> right) : Specification<T>
{
    public override string RuleDescription => $"({left.RuleDescription}) AND ({right.RuleDescription})";
    
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = left.ToExpression();
        var rightExpr = right.ToExpression();
        
        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.AndAlso(
            Expression.Invoke(leftExpr, parameter),
            Expression.Invoke(rightExpr, parameter)
        );
        
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}

/// <summary>
/// Combines two specifications with logical OR
/// </summary>
public class OrSpecification<T>(Specification<T> left, Specification<T> right) : Specification<T>
{
    public override string RuleDescription => $"({left.RuleDescription}) OR ({right.RuleDescription})";
    
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = left.ToExpression();
        var rightExpr = right.ToExpression();
        
        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.OrElse(
            Expression.Invoke(leftExpr, parameter),
            Expression.Invoke(rightExpr, parameter)
        );
        
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}

/// <summary>
/// Negates a specification
/// </summary>
public class NotSpecification<T>(Specification<T> specification) : Specification<T>
{
    public override string RuleDescription => $"NOT ({specification.RuleDescription})";
    
    public override Expression<Func<T, bool>> ToExpression()
    {
        var expr = specification.ToExpression();
        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.Not(Expression.Invoke(expr, parameter));
        
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}

/// <summary>
/// Result of a business rule check
/// </summary>
public record BusinessRuleResult(bool IsValid, string RuleName, string? ErrorMessage = null)
{
    public static BusinessRuleResult Success(string ruleName) => new(true, ruleName);
    public static BusinessRuleResult Failure(string ruleName, string errorMessage) => new(false, ruleName, errorMessage);
}

/// <summary>
/// Interface for business rules that can be checked on aggregates
/// </summary>
public interface IBusinessRule
{
    string RuleName { get; }
    string ErrorMessage { get; }
    bool IsBroken();
}

/// <summary>
/// Extension methods for specification validation
/// </summary>
public static class SpecificationExtensions
{
    /// <summary>
    /// Validates an entity against a specification and throws if not satisfied
    /// </summary>
    public static void CheckRule<T>(this T entity, ISpecification<T> specification)
    {
        if (!specification.IsSatisfiedBy(entity))
        {
            throw new DomainException($"Business rule violated: {specification.RuleDescription}");
        }
    }
    
    /// <summary>
    /// Validates an entity against a specification and returns the result
    /// </summary>
    public static BusinessRuleResult Validate<T>(this T entity, ISpecification<T> specification)
    {
        return specification.IsSatisfiedBy(entity)
            ? BusinessRuleResult.Success(specification.RuleDescription)
            : BusinessRuleResult.Failure(specification.RuleDescription, $"Business rule violated: {specification.RuleDescription}");
    }
    
    /// <summary>
    /// Validates an entity against multiple specifications
    /// </summary>
    public static IEnumerable<BusinessRuleResult> ValidateAll<T>(this T entity, params ISpecification<T>[] specifications)
    {
        return specifications.Select(spec => entity.Validate(spec));
    }
}
