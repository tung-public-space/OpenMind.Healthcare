using Microsoft.EntityFrameworkCore;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Domain.Repositories;

namespace QuitSmokingApi.Infrastructure.Data.Repositories;

/// <summary>
/// Repository implementation for the Achievement aggregate root.
/// </summary>
public class AchievementRepository : IAchievementRepository
{
    private readonly AppDbContext _context;

    public AchievementRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Achievement>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Achievements.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Achievement>> GetAllOrderedByRequiredDaysAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Achievements
            .OrderBy(a => a.RequiredDays)
            .ToListAsync(cancellationToken);
    }
}
