namespace DDD.BuildingBlocks.Specifications;

/// <summary>
/// Result of a business rule check
/// </summary>
public record BusinessRuleResult(bool IsValid, string RuleName, string? ErrorMessage = null)
{
    public static BusinessRuleResult Success(string ruleName) => new(true, ruleName);
    public static BusinessRuleResult Failure(string ruleName, string errorMessage) => new(false, ruleName, errorMessage);
}
