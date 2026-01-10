using MediatR;
using QuitSmokingApi.Features.Progress.Domain;

namespace QuitSmokingApi.Features.Progress.GetHealthMilestones;

public record GetHealthMilestonesQuery : IRequest<List<HealthMilestone>>;
