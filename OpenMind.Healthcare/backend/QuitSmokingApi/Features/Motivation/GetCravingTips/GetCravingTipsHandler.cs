using MediatR;
using QuitSmokingApi.Infrastructure.Data;

namespace QuitSmokingApi.Features.Motivation.GetCravingTips;

public class GetCravingTipsHandler : IRequestHandler<GetCravingTipsQuery, List<CravingTipDto>>
{
    private readonly AppDbContext _context;

    public GetCravingTipsHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<CravingTipDto>> Handle(GetCravingTipsQuery request, CancellationToken cancellationToken)
    {
        var tips = _context.CravingTips.ToList()
            .Select(t => new CravingTipDto(
                t.Id,
                t.Title,
                t.Description,
                t.Icon,
                t.Category.ToString()
            ))
            .ToList();
        return Task.FromResult(tips);
    }
}
