using MediatR;
using Microsoft.EntityFrameworkCore;
using QuitSmokingApi.Infrastructure.Data;
using QuitSmokingApi.Services;

namespace QuitSmokingApi.Features.Achievements.GetAllAchievements;

/// <summary>
/// Handler that uses domain model for achievement calculations
/// </summary>
public class GetAllAchievementsHandler : IRequestHandler<GetAllAchievementsQuery, List<AchievementDto>>
{
    private readonly AppDbContext _context;
    private readonly UserService _userService;

    public GetAllAchievementsHandler(AppDbContext context, UserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<List<AchievementDto>> Handle(GetAllAchievementsQuery request, CancellationToken cancellationToken)
    {
        var userId = _userService.GetCurrentUserId();
        
        var achievements = await _context.Achievements.OrderBy(a => a.RequiredDays).ToListAsync(cancellationToken);
        var journey = await _context.QuitJourneys.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
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

        return dtos;
    }
}
