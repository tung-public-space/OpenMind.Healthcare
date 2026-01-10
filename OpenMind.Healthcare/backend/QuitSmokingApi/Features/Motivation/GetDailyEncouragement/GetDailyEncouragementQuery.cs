using MediatR;
using QuitSmokingApi.Features.Motivation.Domain;

namespace QuitSmokingApi.Features.Motivation.GetDailyEncouragement;

public record GetDailyEncouragementQuery : IRequest<DailyEncouragement>;
