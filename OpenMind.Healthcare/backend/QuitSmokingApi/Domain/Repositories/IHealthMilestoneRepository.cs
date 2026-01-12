using QuitSmokingApi.Domain.Aggregates;

namespace QuitSmokingApi.Domain.Repositories;

public interface IHealthMilestoneRepository
{
    Task<IReadOnlyList<HealthMilestone>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<HealthMilestone>> GetAllOrderedByTimeRequiredAsync(CancellationToken cancellationToken = default);
}
