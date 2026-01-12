using MediatR;
using QuitSmokingApi.Domain.Repositories;

namespace QuitSmokingApi.Features.Motivation.GetRandomQuote;

public class GetRandomQuoteHandler : IRequestHandler<GetRandomQuoteQuery, MotivationalQuoteDto>
{
    private readonly IMotivationalQuoteRepository _motivationalQuoteRepository;

    public GetRandomQuoteHandler(IMotivationalQuoteRepository motivationalQuoteRepository)
    {
        _motivationalQuoteRepository = motivationalQuoteRepository;
    }

    public async Task<MotivationalQuoteDto> Handle(GetRandomQuoteQuery request, CancellationToken cancellationToken)
    {
        var quotes = await _motivationalQuoteRepository.GetAllAsync(cancellationToken);
        var random = new Random();
        var quote = quotes[random.Next(quotes.Count)];
        
        return new MotivationalQuoteDto(
            Id: quote.Id,
            Quote: quote.Quote,
            Author: quote.Author,
            Category: quote.Category.ToString()
        );
    }
}
