using MediatR;
using QuitSmokingApi.Domain.Services;
using QuitSmokingApi.Features.Motivation.GetCravingTips;
using QuitSmokingApi.Features.Motivation.GetRandomQuote;
using QuitSmokingApi.Features.Progress.GetStats;
using QuitSmokingApi.Infrastructure.Data;

namespace QuitSmokingApi.Features.Motivation.GetDailyEncouragement;

/// <summary>
/// Handler that uses the domain EncouragementService for generating messages
/// </summary>
public class GetDailyEncouragementHandler(IMediator mediator)
    : IRequestHandler<GetDailyEncouragementQuery, DailyEncouragementDto>
{
    private readonly EncouragementService _encouragementService = new();

    public async Task<DailyEncouragementDto> Handle(GetDailyEncouragementQuery request, CancellationToken cancellationToken)
    {
        var stats = await mediator.Send(new GetStatsQuery(), cancellationToken);
        var quote = await mediator.Send(new GetRandomQuoteQuery(), cancellationToken);
        var allTips = await mediator.Send(new GetCravingTipsQuery(), cancellationToken);
        
        // Get 3 random tips
        var random = new Random();
        var randomTips = allTips.OrderBy(_ => random.Next()).Take(3).ToList();

        // Use domain service for generating messages
        var message = _encouragementService.GenerateEncouragementMessage(stats);
        var specialMessage = _encouragementService.GenerateSpecialMilestoneMessage(stats.DaysSmokeFree);
        var cravingEncouragement = _encouragementService.GenerateCravingEncouragement(stats.DaysSmokeFree);

        return new DailyEncouragementDto(
            Message: message,
            SpecialMessage: specialMessage,
            Quote: quote,
            Tips: randomTips,
            CravingEncouragement: cravingEncouragement
        );
    }
}
