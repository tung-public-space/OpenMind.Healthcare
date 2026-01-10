using MediatR;
using QuitSmokingApi.Features.Motivation.Domain;
using QuitSmokingApi.Infrastructure.Data;

namespace QuitSmokingApi.Features.Motivation.GetRandomQuote;

public class GetRandomQuoteHandler : IRequestHandler<GetRandomQuoteQuery, MotivationalQuote>
{
    private readonly AppDbContext _context;

    public GetRandomQuoteHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<MotivationalQuote> Handle(GetRandomQuoteQuery request, CancellationToken cancellationToken)
    {
        var quotes = _context.MotivationalQuotes.ToList();
        var random = new Random();
        return Task.FromResult(quotes[random.Next(quotes.Count)]);
    }
}
