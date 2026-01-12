using MediatR;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Domain.Repositories;
using QuitSmokingApi.Services;

namespace QuitSmokingApi.Features.Progress.GetProgress;

public record GetProgressQuery : IRequest<QuitJourney?>;

public class GetProgressHandler(
    IQuitJourneyRepository journeyRepository,
    IUserService userService) : IRequestHandler<GetProgressQuery, QuitJourney?>
{
    public async Task<QuitJourney?> Handle(GetProgressQuery request, CancellationToken cancellationToken)
    {
        var userId = userService.GetCurrentUserId() 
            ?? throw new UnauthorizedAccessException("User not authenticated");

        return await journeyRepository.GetByUserIdAsync(userId, cancellationToken);
    }
}
