using MediatR;
using QuitSmokingApi.Domain.Aggregates;

namespace QuitSmokingApi.Features.Progress.GetStats;

public record GetStatsQuery : IRequest<ProgressStatistics>;
