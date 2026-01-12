using MediatR;
using QuitSmokingApi.Domain.Repositories;

namespace QuitSmokingApi.Features.Motivation.GetCravingTips;

public class GetCravingTipsHandler : IRequestHandler<GetCravingTipsQuery, List<CravingTipDto>>
{
    private readonly ICravingTipRepository _cravingTipRepository;

    public GetCravingTipsHandler(ICravingTipRepository cravingTipRepository)
    {
        _cravingTipRepository = cravingTipRepository;
    }

    public async Task<List<CravingTipDto>> Handle(GetCravingTipsQuery request, CancellationToken cancellationToken)
    {
        var tips = await _cravingTipRepository.GetAllAsync(cancellationToken);
        return tips
            .Select(t => new CravingTipDto(
                t.Id,
                t.Title,
                t.Description,
                t.Icon,
                t.Category.ToString()
            ))
            .ToList();
    }
}
