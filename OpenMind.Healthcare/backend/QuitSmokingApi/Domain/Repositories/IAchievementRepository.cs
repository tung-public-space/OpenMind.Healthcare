using QuitSmokingApi.Domain.Aggregates;

namespace QuitSmokingApi.Domain.Repositories;

/// <summary>
/// Repository interface for the Achievement aggregate root.
/// </summary>
public interface IAchievementRepository
{
    Task<IReadOnlyList<Achievement>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Achievement>> GetAllOrderedByRequiredDaysAsync(CancellationToken cancellationToken = default);
}
