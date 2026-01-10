using MediatR;
using QuitSmokingApi.Domain.Aggregates;

namespace QuitSmokingApi.Features.Progress.GetProgress;

public record GetProgressQuery : IRequest<QuitJourney?>;
