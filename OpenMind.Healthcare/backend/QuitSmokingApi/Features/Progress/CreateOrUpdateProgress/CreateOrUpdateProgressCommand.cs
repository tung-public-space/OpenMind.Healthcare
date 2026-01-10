using MediatR;
using QuitSmokingApi.Domain.Aggregates;

namespace QuitSmokingApi.Features.Progress.CreateOrUpdateProgress;

public record CreateOrUpdateProgressCommand(
    DateTime QuitDate,
    int CigarettesPerDay,
    decimal PricePerPack,
    int CigarettesPerPack
) : IRequest<QuitJourney>;
