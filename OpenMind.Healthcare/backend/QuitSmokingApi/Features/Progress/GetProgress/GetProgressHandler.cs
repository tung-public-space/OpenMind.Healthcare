using MediatR;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Domain.Repositories;
using QuitSmokingApi.Services;

namespace QuitSmokingApi.Features.Progress.GetProgress;

public class GetProgressHandler : IRequestHandler<GetProgressQuery, QuitJourney?>
{
    private readonly IQuitJourneyRepository _journeyRepository;
    private readonly IUserService _userService;

    public GetProgressHandler(IQuitJourneyRepository journeyRepository, IUserService userService)
    {
        _journeyRepository = journeyRepository;
        _userService = userService;
    }

    public async Task<QuitJourney?> Handle(GetProgressQuery request, CancellationToken cancellationToken)
    {
        var userId = _userService.GetCurrentUserId() 
            ?? throw new UnauthorizedAccessException("User not authenticated");

        return await _journeyRepository.GetByUserIdAsync(userId, cancellationToken);
    }
}
