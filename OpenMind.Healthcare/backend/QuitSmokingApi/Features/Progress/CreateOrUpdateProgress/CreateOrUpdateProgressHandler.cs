using MediatR;
using QuitSmokingApi.Domain.Aggregates;
using QuitSmokingApi.Infrastructure.Data;

namespace QuitSmokingApi.Features.Progress.CreateOrUpdateProgress;

public class CreateOrUpdateProgressHandler : IRequestHandler<CreateOrUpdateProgressCommand, QuitJourney>
{
    private readonly AppDbContext _context;

    public CreateOrUpdateProgressHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<QuitJourney> Handle(CreateOrUpdateProgressCommand request, CancellationToken cancellationToken)
    {
        var existing = _context.QuitJourneys.FirstOrDefault();
        
        if (existing != null)
        {
            // Use domain method to update
            existing.Update(
                request.QuitDate, 
                request.CigarettesPerDay, 
                request.CigarettesPerPack, 
                request.PricePerPack);
            _context.SaveChanges();
            return Task.FromResult(existing);
        }

        // Use factory method to create new journey
        var journey = QuitJourney.Start(
            request.QuitDate,
            request.CigarettesPerDay,
            request.CigarettesPerPack,
            request.PricePerPack);
        
        _context.QuitJourneys.Add(journey);
        _context.SaveChanges();
        return Task.FromResult(journey);
    }
}
