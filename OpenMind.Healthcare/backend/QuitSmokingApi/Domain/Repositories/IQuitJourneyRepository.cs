using QuitSmokingApi.Domain.Aggregates;

namespace QuitSmokingApi.Domain.Repositories;

/// <summary>
/// Repository interface for the QuitJourney aggregate root.
/// </summary>
public interface IQuitJourneyRepository
{
    Task<QuitJourney?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(QuitJourney journey, CancellationToken cancellationToken = default);
    Task UpdateAsync(QuitJourney journey, CancellationToken cancellationToken = default);
}
