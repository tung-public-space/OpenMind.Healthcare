using MediatR;
using QuitSmokingApi.Infrastructure.Data;

namespace QuitSmokingApi.Features.Achievements.GetAllAchievements;

/// <summary>
/// Handler that uses domain model for achievement calculations
/// </summary>
public class GetAllAchievementsHandler : IRequestHandler<GetAllAchievementsQuery, List<AchievementDto>>
{
    private readonly AppDbContext _context;

    public GetAllAchievementsHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<AchievementDto>> Handle(GetAllAchievementsQuery request, CancellationToken cancellationToken)
    {
        var achievements = _context.Achievements.OrderBy(a => a.RequiredDays).ToList();
        var journey = _context.QuitJourneys.FirstOrDefault();
        var daysSinceQuit = journey?.GetDaysSmokeFree() ?? 0;

        var dtos = achievements.Select(a => new AchievementDto(
            Id: a.Id,
            Name: a.Name,
            Description: a.Description,
            Icon: a.Icon,
            RequiredDays: a.RequiredDays,
            Category: a.Category.ToString(),
            IsUnlocked: a.IsUnlockedFor(daysSinceQuit),
            UnlockedAt: a.IsUnlockedFor(daysSinceQuit) && journey != null 
                ? journey.QuitDate.AddDays(a.RequiredDays) 
                : null,
            ProgressPercentage: Math.Round(a.GetProgress(daysSinceQuit), 2)
        )).ToList();

        return Task.FromResult(dtos);
    }
}
