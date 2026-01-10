using MediatR;

namespace QuitSmokingApi.Features.Motivation.GetCravingTips;

public record GetCravingTipsQuery : IRequest<List<CravingTipDto>>;
