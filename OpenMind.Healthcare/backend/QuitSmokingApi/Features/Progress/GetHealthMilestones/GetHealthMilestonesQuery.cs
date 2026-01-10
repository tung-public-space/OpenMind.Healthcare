using MediatR;

namespace QuitSmokingApi.Features.Progress.GetHealthMilestones;

public record GetHealthMilestonesQuery : IRequest<List<HealthMilestoneDto>>;
