using DDD.BuildingBlocks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserApi.Domain;

namespace UserApi.Infrastructure;

public class UserDbContext(DbContextOptions<UserDbContext> options, IMediator mediator) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker.Entries<Entity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        foreach (var entity in entities)
        {
            entity.ClearDomainEvents();
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Ignore(u => u.DomainEvents);
            
            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);
            
            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.Property(u => u.FirstName)
                .HasMaxLength(100);

            entity.Property(u => u.LastName)
                .HasMaxLength(100);

            entity.Property(u => u.CreatedAt)
                .IsRequired();

            entity.Property(u => u.UpdatedAt);
        });
    }
}
