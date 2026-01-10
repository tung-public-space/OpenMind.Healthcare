using MediatR;
using QuitSmokingApi.Features.Progress.Domain;
using QuitSmokingApi.Infrastructure.Data;

namespace QuitSmokingApi.Features.Progress.CreateOrUpdateProgress;

public class CreateOrUpdateProgressHandler : IRequestHandler<CreateOrUpdateProgressCommand, UserProgress>
{
    private readonly AppDbContext _context;

    public CreateOrUpdateProgressHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<UserProgress> Handle(CreateOrUpdateProgressCommand request, CancellationToken cancellationToken)
    {
        var existing = _context.UserProgress.FirstOrDefault();
        
        if (existing != null)
        {
            existing.QuitDate = request.QuitDate;
            existing.CigarettesPerDay = request.CigarettesPerDay;
            existing.PricePerPack = request.PricePerPack;
            existing.CigarettesPerPack = request.CigarettesPerPack;
            existing.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();
            return Task.FromResult(existing);
        }

        var progress = new UserProgress
        {
            QuitDate = request.QuitDate,
            CigarettesPerDay = request.CigarettesPerDay,
            PricePerPack = request.PricePerPack,
            CigarettesPerPack = request.CigarettesPerPack,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _context.UserProgress.Add(progress);
        _context.SaveChanges();
        return Task.FromResult(progress);
    }
}
