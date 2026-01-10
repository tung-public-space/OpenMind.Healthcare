using MediatR;
using QuitSmokingApi.Features.Progress.Domain;

namespace QuitSmokingApi.Features.Progress.CreateOrUpdateProgress;

public record CreateOrUpdateProgressCommand(
    DateTime QuitDate,
    int CigarettesPerDay,
    decimal PricePerPack,
    int CigarettesPerPack
) : IRequest<UserProgress>;
