using MediatR;
using QuitSmokingApi.Features.Achievements.GetAllAchievements;
using QuitSmokingApi.Features.Achievements.Specifications;

namespace QuitSmokingApi.Features.Achievements.GetUnlockedAchievements;

public record GetUnlockedAchievementsQuery : IRequest<List<AchievementDto>>;

public class GetUnlockedAchievementsHandler(IMediator mediator) 
    : IRequestHandler<GetUnlockedAchievementsQuery, List<AchievementDto>>
{
    public async Task<List<AchievementDto>> Handle(GetUnlockedAchievementsQuery request, CancellationToken cancellationToken)
    {
        var all = await mediator.Send(new GetAllAchievementsQuery(), cancellationToken);
        
        // Use application layer specification for filtering
        var unlockedSpec = AchievementSpecs.Unlocked();
        
        return all.Where(unlockedSpec.ToExpression().Compile()).ToList();
    }
}
