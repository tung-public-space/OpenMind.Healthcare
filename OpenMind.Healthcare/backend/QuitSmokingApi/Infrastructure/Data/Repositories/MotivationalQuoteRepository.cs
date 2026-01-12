using Microsoft.EntityFrameworkCore;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Domain.Repositories;

namespace QuitSmokingApi.Infrastructure.Data.Repositories;

/// <summary>
/// Repository implementation for the MotivationalQuote aggregate root.
/// </summary>
public class MotivationalQuoteRepository(AppDbContext context) : IMotivationalQuoteRepository
{
    public async Task<IReadOnlyList<MotivationalQuote>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.MotivationalQuotes.ToListAsync(cancellationToken);
    }
}
