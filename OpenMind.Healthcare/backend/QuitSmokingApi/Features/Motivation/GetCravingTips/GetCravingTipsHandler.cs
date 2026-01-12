using MediatR;
using QuitSmokingApi.Domain.Repositories;

namespace QuitSmokingApi.Features.Motivation.GetCravingTips;

public record GetCravingTipsQuery : IRequest<List<CravingTipDto>>;

public class GetCravingTipsHandler(
    ICravingTipRepository cravingTipRepository) : IRequestHandler<GetCravingTipsQuery, List<CravingTipDto>>
{
    public async Task<List<CravingTipDto>> Handle(GetCravingTipsQuery request, CancellationToken cancellationToken)
    {
        var tips = await cravingTipRepository.GetAllAsync(cancellationToken);
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
