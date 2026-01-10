using MediatR;
using QuitSmokingApi.Features.Motivation.Domain;

namespace QuitSmokingApi.Features.Motivation.GetCravingTips;

public record GetCravingTipsQuery : IRequest<List<CravingTip>>;
