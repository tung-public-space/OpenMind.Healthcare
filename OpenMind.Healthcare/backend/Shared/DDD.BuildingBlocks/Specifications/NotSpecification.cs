using System.Linq.Expressions;

namespace DDD.BuildingBlocks.Specifications;

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
