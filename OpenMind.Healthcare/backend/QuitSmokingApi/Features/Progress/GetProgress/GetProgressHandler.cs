using MediatR;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Infrastructure.Data;

namespace QuitSmokingApi.Features.Progress.GetProgress;

public class GetProgressHandler : IRequestHandler<GetProgressQuery, QuitJourney?>
{
    private readonly AppDbContext _context;

    public GetProgressHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<QuitJourney?> Handle(GetProgressQuery request, CancellationToken cancellationToken)
    {
        var journey = _context.QuitJourneys.FirstOrDefault();
        return Task.FromResult(journey);
    }
}
