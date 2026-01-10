using MediatR;
using QuitSmokingApi.Features.Progress.Domain;
using QuitSmokingApi.Infrastructure.Data;

namespace QuitSmokingApi.Features.Progress.GetProgress;

public class GetProgressHandler : IRequestHandler<GetProgressQuery, UserProgress?>
{
    private readonly AppDbContext _context;

    public GetProgressHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<UserProgress?> Handle(GetProgressQuery request, CancellationToken cancellationToken)
    {
        var progress = _context.UserProgress.FirstOrDefault();
        return Task.FromResult(progress);
    }
}
