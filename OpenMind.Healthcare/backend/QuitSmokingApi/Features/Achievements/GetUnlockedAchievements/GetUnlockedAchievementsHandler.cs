using MediatR;
using QuitSmokingApi.Features.Achievements.GetAllAchievements;

namespace QuitSmokingApi.Features.Achievements.GetUnlockedAchievements;

public record GetUnlockedAchievementsQuery : IRequest<List<AchievementDto>>;

public class GetUnlockedAchievementsHandler(IMediator mediator) 
    : IRequestHandler<GetUnlockedAchievementsQuery, List<AchievementDto>>
{
    public async Task<List<AchievementDto>> Handle(GetUnlockedAchievementsQuery request, CancellationToken cancellationToken)
    {
        var all = await mediator.Send(new GetAllAchievementsQuery(), cancellationToken);
        return all.Where(a => a.IsUnlocked).ToList();
    }
}
