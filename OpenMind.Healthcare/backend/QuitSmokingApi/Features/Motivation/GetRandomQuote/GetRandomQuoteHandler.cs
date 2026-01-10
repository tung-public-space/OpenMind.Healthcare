using MediatR;
using QuitSmokingApi.Infrastructure.Data;

namespace QuitSmokingApi.Features.Motivation.GetRandomQuote;

public class GetRandomQuoteHandler : IRequestHandler<GetRandomQuoteQuery, MotivationalQuoteDto>
{
    private readonly AppDbContext _context;

    public GetRandomQuoteHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<MotivationalQuoteDto> Handle(GetRandomQuoteQuery request, CancellationToken cancellationToken)
    {
        var quotes = _context.MotivationalQuotes.ToList();
        var random = new Random();
        var quote = quotes[random.Next(quotes.Count)];
        
        return Task.FromResult(new MotivationalQuoteDto(
            Id: quote.Id,
            Quote: quote.Quote,
            Author: quote.Author,
            Category: quote.Category.ToString()
        ));
    }
}
