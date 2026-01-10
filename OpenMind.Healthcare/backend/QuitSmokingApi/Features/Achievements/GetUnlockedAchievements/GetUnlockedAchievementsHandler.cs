using MediatR;
using QuitSmokingApi.Features.Achievements.GetAllAchievements;

namespace QuitSmokingApi.Features.Achievements.GetUnlockedAchievements;

public class GetUnlockedAchievementsHandler : IRequestHandler<GetUnlockedAchievementsQuery, List<AchievementDto>>
{
    private readonly IMediator _mediator;

    public GetUnlockedAchievementsHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<List<AchievementDto>> Handle(GetUnlockedAchievementsQuery request, CancellationToken cancellationToken)
    {
        var all = await _mediator.Send(new GetAllAchievementsQuery(), cancellationToken);
        return all.Where(a => a.IsUnlocked).ToList();
    }
}
