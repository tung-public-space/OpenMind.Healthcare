using MediatR;
using QuitSmokingApi.Domain.ValueObjects;

namespace QuitSmokingApi.Features.Progress.GetStats;

public record GetStatsQuery : IRequest<ProgressStatistics>;
