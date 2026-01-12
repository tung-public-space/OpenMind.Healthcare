using System.Linq.Expressions;

namespace DDD.BuildingBlocks.Specifications;

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
