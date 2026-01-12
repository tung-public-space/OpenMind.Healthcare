using System.Linq.Expressions;

namespace DDD.BuildingBlocks.Specifications;

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
