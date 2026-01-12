using MediatR;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Domain.Repositories;
using QuitSmokingApi.Services;

namespace QuitSmokingApi.Features.Progress.CreateOrUpdateProgress;

/// <summary>
/// Handler for creating or updating a quit journey.
/// Uses repository for data access following DDD principles.
/// </summary>
public class CreateOrUpdateProgressHandler : IRequestHandler<CreateOrUpdateProgressCommand, QuitJourney>
{
    private readonly IQuitJourneyRepository _journeyRepository;
    private readonly IUserService _userService;

    public CreateOrUpdateProgressHandler(IQuitJourneyRepository journeyRepository, IUserService userService)
    {
        _journeyRepository = journeyRepository;
        _userService = userService;
    }

    public async Task<QuitJourney> Handle(CreateOrUpdateProgressCommand request, CancellationToken cancellationToken)
    {
        var userId = _userService.GetCurrentUserId() 
            ?? throw new UnauthorizedAccessException("User not authenticated");

        var existing = await _journeyRepository.GetByUserIdAsync(userId, cancellationToken);
        
        if (existing != null)
        {
            // Use domain method to update
            existing.Update(
                request.QuitDate, 
                request.CigarettesPerDay, 
                request.CigarettesPerPack, 
                request.PricePerPack);
            await _journeyRepository.UpdateAsync(existing, cancellationToken);
            return existing;
        }

        // Use factory method to create new journey
        var journey = QuitJourney.Start(
            userId,
            request.QuitDate,
            request.CigarettesPerDay,
            request.CigarettesPerPack,
            request.PricePerPack);
        
        await _journeyRepository.AddAsync(journey, cancellationToken);
        return journey;
    }
}
