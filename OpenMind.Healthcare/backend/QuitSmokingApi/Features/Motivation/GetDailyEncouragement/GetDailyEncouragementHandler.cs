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
public class GetDailyEncouragementHandler : IRequestHandler<GetDailyEncouragementQuery, DailyEncouragementDto>
{
    private readonly AppDbContext _context;
    private readonly IMediator _mediator;
    private readonly EncouragementService _encouragementService;

    public GetDailyEncouragementHandler(AppDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
        _encouragementService = new EncouragementService();
    }

    public async Task<DailyEncouragementDto> Handle(GetDailyEncouragementQuery request, CancellationToken cancellationToken)
    {
        var stats = await _mediator.Send(new GetStatsQuery(), cancellationToken);
        var quote = await _mediator.Send(new GetRandomQuoteQuery(), cancellationToken);
        var allTips = await _mediator.Send(new GetCravingTipsQuery(), cancellationToken);
        
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
