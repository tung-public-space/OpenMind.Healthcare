using MediatR;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Domain.Repositories;
using QuitSmokingApi.Services;

namespace QuitSmokingApi.Features.Progress.CreateOrUpdateProgress;

public record CreateOrUpdateProgressCommand(
    DateTime QuitDate,
    int CigarettesPerDay,
    decimal PricePerPack,
    int CigarettesPerPack
) : IRequest<QuitJourney>;

public class CreateOrUpdateProgressHandler(
    IQuitJourneyRepository journeyRepository,
    IUserService userService) : IRequestHandler<CreateOrUpdateProgressCommand, QuitJourney>
{
    public async Task<QuitJourney> Handle(CreateOrUpdateProgressCommand request, CancellationToken cancellationToken)
    {
        var userId = userService.GetCurrentUserId() 
            ?? throw new UnauthorizedAccessException("User not authenticated");

        var existing = await journeyRepository.GetByUserIdAsync(userId, cancellationToken);
        
        if (existing != null)
        {
            existing.Update(
                request.QuitDate, 
                request.CigarettesPerDay, 
                request.CigarettesPerPack, 
                request.PricePerPack);
            await journeyRepository.UpdateAsync(existing, cancellationToken);
            return existing;
        }

        var journey = QuitJourney.Start(
            userId,
            request.QuitDate,
            request.CigarettesPerDay,
            request.CigarettesPerPack,
            request.PricePerPack);
        
        await journeyRepository.AddAsync(journey, cancellationToken);
        return journey;
    }
}
