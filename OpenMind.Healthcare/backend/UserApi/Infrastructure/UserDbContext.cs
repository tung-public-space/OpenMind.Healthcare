using DDD.BuildingBlocks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserApi.Domain;

namespace UserApi.Infrastructure;

public class UserDbContext(DbContextOptions<UserDbContext> options, IMediator mediator) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

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

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(rt => rt.Id);
            entity.Ignore(rt => rt.DomainEvents);

            entity.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(256);

            entity.HasIndex(rt => rt.Token)
                .IsUnique();

            entity.Property(rt => rt.UserId)
                .IsRequired();

            entity.HasIndex(rt => rt.UserId);

            entity.Property(rt => rt.ExpiresAt)
                .IsRequired();

            entity.Property(rt => rt.CreatedByIp)
                .HasMaxLength(50);

            entity.Property(rt => rt.RevokedByIp)
                .HasMaxLength(50);

            entity.Property(rt => rt.ReplacedByToken)
                .HasMaxLength(256);

            entity.Property(rt => rt.CreatedAt)
                .IsRequired();

            entity.Property(rt => rt.UpdatedAt);
        });
    }
}
