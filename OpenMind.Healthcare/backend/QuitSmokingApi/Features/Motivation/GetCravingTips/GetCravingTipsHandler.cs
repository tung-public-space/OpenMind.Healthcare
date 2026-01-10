using MediatR;
using QuitSmokingApi.Features.Motivation.Domain;
using QuitSmokingApi.Infrastructure.Data;

namespace QuitSmokingApi.Features.Motivation.GetCravingTips;

public class GetCravingTipsHandler : IRequestHandler<GetCravingTipsQuery, List<CravingTip>>
{
    private readonly AppDbContext _context;

    public GetCravingTipsHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<CravingTip>> Handle(GetCravingTipsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_context.CravingTips.ToList());
    }
}
