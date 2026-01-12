using Microsoft.EntityFrameworkCore;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Domain.Repositories;

namespace QuitSmokingApi.Infrastructure.Data.Repositories;

/// <summary>
/// Repository implementation for the MotivationalQuote aggregate root.
/// </summary>
public class MotivationalQuoteRepository : IMotivationalQuoteRepository
{
    private readonly AppDbContext _context;

    public MotivationalQuoteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<MotivationalQuote>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.MotivationalQuotes.ToListAsync(cancellationToken);
    }
}
