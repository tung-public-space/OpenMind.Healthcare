using QuitSmokingApi.Domain.Aggregates;

namespace QuitSmokingApi.Domain.Repositories;

/// <summary>
/// Repository interface for the CravingTip aggregate root.
/// </summary>
public interface ICravingTipRepository
{
    Task<IReadOnlyList<CravingTip>> GetAllAsync(CancellationToken cancellationToken = default);
}
