using QuitSmokingApi.Domain.Aggregates;

namespace QuitSmokingApi.Domain.Repositories;

/// <summary>
/// Repository interface for the MotivationalQuote aggregate root.
/// </summary>
public interface IMotivationalQuoteRepository
{
    Task<IReadOnlyList<MotivationalQuote>> GetAllAsync(CancellationToken cancellationToken = default);
}
