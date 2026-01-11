using MediatR;
using Microsoft.EntityFrameworkCore;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Domain.ValueObjects;
using QuitSmokingApi.Infrastructure.Data;
using QuitSmokingApi.Services;
using DomainHealthMilestone = QuitSmokingApi.Domain.Aggregates.HealthMilestone;

namespace QuitSmokingApi.Features.Progress.GetHealthMilestones;

/// <summary>
/// Handler that leverages the rich domain model for health milestones
/// The milestone definitions and progress calculations are in the domain
/// </summary>
public class GetHealthMilestonesHandler : IRequestHandler<GetHealthMilestonesQuery, List<HealthMilestoneDto>>
{
    private readonly AppDbContext _context;
    private readonly UserService _userService;

    public GetHealthMilestonesHandler(AppDbContext context, UserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<List<HealthMilestoneDto>> Handle(GetHealthMilestonesQuery request, CancellationToken cancellationToken)
    {
        var userId = _userService.GetCurrentUserId();
        
        var journey = await _context.QuitJourneys.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        var timeSinceQuit = journey?.GetTimeSinceQuit() ?? Duration.Zero;

        // Map domain health milestones to DTOs with progress calculated
        var milestones = DomainHealthMilestone.GetAll()
            .Select((m, index) => new HealthMilestoneDto(
                Id: GenerateDeterministicGuid(m.Title),
                Title: m.Title,
                Description: m.Description,
                TimeInMinutes: m.TimeRequired.TotalMinutes,
                TimeDisplay: m.TimeDisplay,
                Icon: m.Icon,
                Category: m.Category.ToString(),
                IsAchieved: m.IsAchieved(timeSinceQuit),
                ProgressPercentage: Math.Round(m.GetProgress(timeSinceQuit), 2)
            ))
            .ToList();

        return milestones;
    }
    
    private static Guid GenerateDeterministicGuid(string input)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
        return new Guid(hash);
    }
}
