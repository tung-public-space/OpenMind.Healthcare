using MediatR;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Infrastructure.Data;
using QuitSmokingApi.Services;
using Microsoft.EntityFrameworkCore;

namespace QuitSmokingApi.Features.Progress.CreateOrUpdateProgress;

public class CreateOrUpdateProgressHandler : IRequestHandler<CreateOrUpdateProgressCommand, QuitJourney>
{
    private readonly AppDbContext _context;
    private readonly IUserService _userService;

    public CreateOrUpdateProgressHandler(AppDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<QuitJourney> Handle(CreateOrUpdateProgressCommand request, CancellationToken cancellationToken)
    {
        var userId = _userService.GetCurrentUserId() 
            ?? throw new UnauthorizedAccessException("User not authenticated");

        var existing = await _context.QuitJourneys
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        
        if (existing != null)
        {
            // Use domain method to update
            existing.Update(
                request.QuitDate, 
                request.CigarettesPerDay, 
                request.CigarettesPerPack, 
                request.PricePerPack);
            await _context.SaveChangesAsync(cancellationToken);
            return existing;
        }

        // Use factory method to create new journey
        var journey = QuitJourney.Start(
            userId,
            request.QuitDate,
            request.CigarettesPerDay,
            request.CigarettesPerPack,
            request.PricePerPack);
        
        _context.QuitJourneys.Add(journey);
        await _context.SaveChangesAsync(cancellationToken);
        return journey;
    }
}
