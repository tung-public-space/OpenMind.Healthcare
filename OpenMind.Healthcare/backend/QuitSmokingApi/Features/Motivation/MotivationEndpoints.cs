using MediatR;
using QuitSmokingApi.Features.Motivation.GetCravingTips;
using QuitSmokingApi.Features.Motivation.GetDailyEncouragement;
using QuitSmokingApi.Features.Motivation.GetRandomQuote;

namespace QuitSmokingApi.Features.Motivation;

public static class MotivationEndpoints
{
    public static void MapMotivationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/motivation")
            .WithTags("Motivation")
            .RequireAuthorization();

        group.MapGet("/quote", GetRandomQuote)
            .WithName("GetRandomQuote")
            .WithOpenApi();

        group.MapGet("/craving-tips", GetCravingTips)
            .WithName("GetCravingTips")
            .WithOpenApi();

        group.MapGet("/daily", GetDailyEncouragement)
            .WithName("GetDailyEncouragement")
            .WithOpenApi();
    }

    private static async Task<IResult> GetRandomQuote(IMediator mediator)
    {
        var quote = await mediator.Send(new GetRandomQuoteQuery());
        return Results.Ok(quote);
    }

    private static async Task<IResult> GetCravingTips(IMediator mediator)
    {
        var tips = await mediator.Send(new GetCravingTipsQuery());
        return Results.Ok(tips);
    }

    private static async Task<IResult> GetDailyEncouragement(IMediator mediator)
    {
        var encouragement = await mediator.Send(new GetDailyEncouragementQuery());
        return Results.Ok(encouragement);
    }
}
