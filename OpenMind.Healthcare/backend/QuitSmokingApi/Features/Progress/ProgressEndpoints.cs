using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuitSmokingApi.Features.Progress.CreateOrUpdateProgress;
using QuitSmokingApi.Features.Progress.GetHealthMilestones;
using QuitSmokingApi.Features.Progress.GetProgress;
using QuitSmokingApi.Features.Progress.GetStats;

namespace QuitSmokingApi.Features.Progress;

public static class ProgressEndpoints
{
    public static void MapProgressEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/progress")
            .WithTags("Progress");

        group.MapGet("/", GetProgress)
            .WithName("GetProgress")
            .WithOpenApi();

        group.MapPost("/", CreateOrUpdateProgress)
            .WithName("CreateOrUpdateProgress")
            .WithOpenApi();

        group.MapGet("/stats", GetStats)
            .WithName("GetProgressStats")
            .WithOpenApi();

        group.MapGet("/health-milestones", GetHealthMilestones)
            .WithName("GetHealthMilestones")
            .WithOpenApi();
    }

    private static async Task<IResult> GetProgress(IMediator mediator)
    {
        var progress = await mediator.Send(new GetProgressQuery());
        return progress is null ? Results.NotFound() : Results.Ok(progress);
    }

    private static async Task<IResult> CreateOrUpdateProgress(
        [FromBody] CreateOrUpdateProgressCommand command,
        IMediator mediator)
    {
        var result = await mediator.Send(command);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetStats(IMediator mediator)
    {
        var stats = await mediator.Send(new GetStatsQuery());
        return Results.Ok(stats);
    }

    private static async Task<IResult> GetHealthMilestones(IMediator mediator)
    {
        var milestones = await mediator.Send(new GetHealthMilestonesQuery());
        return Results.Ok(milestones);
    }
}
