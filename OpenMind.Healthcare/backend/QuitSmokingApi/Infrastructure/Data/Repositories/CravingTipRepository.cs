using Microsoft.EntityFrameworkCore;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Domain.Repositories;

namespace QuitSmokingApi.Infrastructure.Data.Repositories;

/// <summary>
/// Repository implementation for the CravingTip aggregate root.
/// </summary>
public class CravingTipRepository : ICravingTipRepository
{
    private readonly AppDbContext _context;

    public CravingTipRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<CravingTip>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CravingTips.ToListAsync(cancellationToken);
    }
}
