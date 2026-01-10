using MediatR;
using QuitSmokingApi.Features.Achievements.CheckNewAchievement;
using QuitSmokingApi.Features.Achievements.GetAllAchievements;
using QuitSmokingApi.Features.Achievements.GetUnlockedAchievements;

namespace QuitSmokingApi.Features.Achievements;

public static class AchievementsEndpoints
{
    public static void MapAchievementsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/achievements")
            .WithTags("Achievements");

        group.MapGet("/", GetAllAchievements)
            .WithName("GetAllAchievements")
            .WithOpenApi();

        group.MapGet("/unlocked", GetUnlockedAchievements)
            .WithName("GetUnlockedAchievements")
            .WithOpenApi();

        group.MapGet("/check-new", CheckForNewAchievement)
            .WithName("CheckForNewAchievement")
            .WithOpenApi();
    }

    private static async Task<IResult> GetAllAchievements(IMediator mediator)
    {
        var achievements = await mediator.Send(new GetAllAchievementsQuery());
        return Results.Ok(achievements);
    }

    private static async Task<IResult> GetUnlockedAchievements(IMediator mediator)
    {
        var achievements = await mediator.Send(new GetUnlockedAchievementsQuery());
        return Results.Ok(achievements);
    }

    private static async Task<IResult> CheckForNewAchievement(IMediator mediator)
    {
        var achievement = await mediator.Send(new CheckNewAchievementQuery());
        return achievement is null ? Results.NoContent() : Results.Ok(achievement);
    }
}
