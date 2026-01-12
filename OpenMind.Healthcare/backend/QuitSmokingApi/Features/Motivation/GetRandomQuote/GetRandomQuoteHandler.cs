using MediatR;
using QuitSmokingApi.Domain.Repositories;

namespace QuitSmokingApi.Features.Motivation.GetRandomQuote;

public record GetRandomQuoteQuery : IRequest<MotivationalQuoteDto>;

public class GetRandomQuoteHandler(
    IMotivationalQuoteRepository motivationalQuoteRepository) : IRequestHandler<GetRandomQuoteQuery, MotivationalQuoteDto>
{
    public async Task<MotivationalQuoteDto> Handle(GetRandomQuoteQuery request, CancellationToken cancellationToken)
    {
        var quotes = await motivationalQuoteRepository.GetAllAsync(cancellationToken);
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
