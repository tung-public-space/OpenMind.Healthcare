using System.Linq.Expressions;
using DDD.BuildingBlocks;
using DDD.BuildingBlocks.Specifications;

namespace QuitSmokingApi.Domain.Specifications;

public class QuitDateNotInFutureSpecification : Specification<DateTime>
{
    public override string RuleDescription => "Quit date cannot be in the future";
    public override Expression<Func<DateTime, bool>> ToExpression() => quitDate => quitDate <= DateTime.UtcNow;
}

public class QuitDateNotTooOldSpecification(int maxYearsAgo = 10) : Specification<DateTime>
{
    public override string RuleDescription => $"Quit date cannot be more than {maxYearsAgo} years in the past";
    public override Expression<Func<DateTime, bool>> ToExpression()
    {
        var minDate = DateTime.UtcNow.AddYears(-maxYearsAgo);
        return quitDate => quitDate >= minDate;
    }
}

public class UserIdNotEmptySpecification : Specification<Guid>
{
    public override string RuleDescription => "User ID cannot be empty";
    public override Expression<Func<Guid, bool>> ToExpression() => userId => userId != Guid.Empty;
}

public static class QuitJourneySpecs
{
    public static QuitDateNotInFutureSpecification QuitDateNotInFuture() => new();
    public static QuitDateNotTooOldSpecification QuitDateNotTooOld(int maxYearsAgo = 10) => new(maxYearsAgo);
    public static UserIdNotEmptySpecification UserIdNotEmpty() => new();
    public static Specification<DateTime> ValidQuitDate(int maxYearsAgo = 10) => QuitDateNotInFuture() & QuitDateNotTooOld(maxYearsAgo);
}
