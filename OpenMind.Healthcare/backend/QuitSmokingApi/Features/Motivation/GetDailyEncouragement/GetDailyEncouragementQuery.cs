using MediatR;

namespace QuitSmokingApi.Features.Motivation.GetDailyEncouragement;

public record GetDailyEncouragementQuery : IRequest<DailyEncouragementDto>;
