using MediatR;
using Microsoft.EntityFrameworkCore;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Infrastructure.Data;
using QuitSmokingApi.Services;

namespace QuitSmokingApi.Features.Progress.GetProgress;

public class GetProgressHandler : IRequestHandler<GetProgressQuery, QuitJourney?>
{
    private readonly AppDbContext _context;
    private readonly IUserService _userService;

    public GetProgressHandler(AppDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<QuitJourney?> Handle(GetProgressQuery request, CancellationToken cancellationToken)
    {
        var userId = _userService.GetCurrentUserId() 
            ?? throw new UnauthorizedAccessException("User not authenticated");

        var journey = await _context.QuitJourneys
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        
        return journey;
    }
}
