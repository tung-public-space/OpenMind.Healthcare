using MediatR;
using QuitSmokingApi.Domain.Repositories;
using QuitSmokingApi.Domain.Services;
using QuitSmokingApi.Services;

namespace QuitSmokingApi.Features.Progress.GetHealthMilestones;

public record GetHealthMilestonesQuery : IRequest<List<HealthMilestoneDto>>;

public class GetHealthMilestonesHandler(
    IHealthMilestoneRepository healthMilestoneRepository,
    IQuitJourneyRepository journeyRepository,
    HealthMilestoneStatusService milestoneStatusService,
    UserService userService) : IRequestHandler<GetHealthMilestonesQuery, List<HealthMilestoneDto>>
{
    public async Task<List<HealthMilestoneDto>> Handle(GetHealthMilestonesQuery request, CancellationToken cancellationToken)
    {
        var userId = userService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException("User not authenticated");
        
        var milestones = await healthMilestoneRepository.GetAllOrderedByTimeRequiredAsync(cancellationToken);
        var journey = await journeyRepository.GetByUserIdAsync(userId, cancellationToken);

        var statuses = milestoneStatusService.ComputeStatuses(milestones, journey);
        
        return statuses
            .Select(s => new HealthMilestoneDto(
                Id: s.MilestoneId,
                Title: s.Title,
                Description: s.Description,
                TimeInMinutes: s.TimeRequiredMinutes,
                TimeDisplay: s.TimeDisplay,
                Icon: s.Icon,
                Category: s.Category,
                IsAchieved: s.IsAchieved,
                ProgressPercentage: s.ProgressPercentage
            ))
            .ToList();
    }
}
