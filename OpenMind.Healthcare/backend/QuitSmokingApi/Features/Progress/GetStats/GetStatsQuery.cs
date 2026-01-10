using MediatR;
using QuitSmokingApi.Features.Progress.Domain;

namespace QuitSmokingApi.Features.Progress.GetStats;

public record GetStatsQuery : IRequest<ProgressStats>;
