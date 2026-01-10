using MediatR;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Domain.ValueObjects;
using QuitSmokingApi.Infrastructure.Data;
using DomainHealthMilestone = QuitSmokingApi.Domain.Aggregates.HealthMilestone;

namespace QuitSmokingApi.Features.Progress.GetHealthMilestones;

/// <summary>
/// Handler that leverages the rich domain model for health milestones
/// The milestone definitions and progress calculations are in the domain
/// </summary>
public class GetHealthMilestonesHandler : IRequestHandler<GetHealthMilestonesQuery, List<HealthMilestoneDto>>
{
    private readonly AppDbContext _context;

    public GetHealthMilestonesHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<HealthMilestoneDto>> Handle(GetHealthMilestonesQuery request, CancellationToken cancellationToken)
    {
        var journey = _context.QuitJourneys.FirstOrDefault();
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

        return Task.FromResult(milestones);
    }
    
    private static Guid GenerateDeterministicGuid(string input)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
        return new Guid(hash);
    }
}
