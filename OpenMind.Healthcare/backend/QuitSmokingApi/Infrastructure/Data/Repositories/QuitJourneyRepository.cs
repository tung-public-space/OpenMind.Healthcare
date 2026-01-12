using Microsoft.EntityFrameworkCore;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Domain.Repositories;

namespace QuitSmokingApi.Infrastructure.Data.Repositories;

/// <summary>
/// Repository implementation for the QuitJourney aggregate root.
/// </summary>
public class QuitJourneyRepository : IQuitJourneyRepository
{
    private readonly AppDbContext _context;

    public QuitJourneyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<QuitJourney?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.QuitJourneys
            .FirstOrDefaultAsync(j => j.UserId == userId, cancellationToken);
    }

    public async Task AddAsync(QuitJourney journey, CancellationToken cancellationToken = default)
    {
        await _context.QuitJourneys.AddAsync(journey, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(QuitJourney journey, CancellationToken cancellationToken = default)
    {
        _context.QuitJourneys.Update(journey);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
