namespace DDD.BuildingBlocks.Specifications;

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
