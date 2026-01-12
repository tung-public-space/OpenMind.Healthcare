using Microsoft.EntityFrameworkCore;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Domain.Repositories;

namespace QuitSmokingApi.Infrastructure.Data.Repositories;

public class HealthMilestoneRepository(AppDbContext context) : IHealthMilestoneRepository
{
    public async Task<IReadOnlyList<HealthMilestone>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.HealthMilestones.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<HealthMilestone>> GetAllOrderedByTimeRequiredAsync(CancellationToken cancellationToken = default)
    {
        return await context.HealthMilestones
            .OrderBy(m => m.TimeRequiredMinutes)
            .ToListAsync(cancellationToken);
    }
}
