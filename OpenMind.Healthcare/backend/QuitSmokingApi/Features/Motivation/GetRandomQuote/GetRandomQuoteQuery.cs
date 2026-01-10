using MediatR;
using QuitSmokingApi.Features.Motivation.Domain;

namespace QuitSmokingApi.Features.Motivation.GetRandomQuote;

public record GetRandomQuoteQuery : IRequest<MotivationalQuote>;
