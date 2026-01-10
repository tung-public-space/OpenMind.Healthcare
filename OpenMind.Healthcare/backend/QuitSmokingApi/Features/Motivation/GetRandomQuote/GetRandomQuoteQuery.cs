using MediatR;

namespace QuitSmokingApi.Features.Motivation.GetRandomQuote;

public record GetRandomQuoteQuery : IRequest<MotivationalQuoteDto>;
