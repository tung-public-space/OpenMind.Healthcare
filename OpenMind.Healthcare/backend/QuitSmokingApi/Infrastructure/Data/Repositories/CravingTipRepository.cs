using Microsoft.EntityFrameworkCore;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Domain.Repositories;

namespace QuitSmokingApi.Infrastructure.Data.Repositories;

/// <summary>
/// Repository implementation for the CravingTip aggregate root.
/// </summary>
public class CravingTipRepository(AppDbContext context) : ICravingTipRepository
{
    public async Task<IReadOnlyList<CravingTip>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.CravingTips.ToListAsync(cancellationToken);
    }
}
