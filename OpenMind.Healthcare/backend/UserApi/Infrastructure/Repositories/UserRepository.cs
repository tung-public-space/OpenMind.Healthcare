using Microsoft.EntityFrameworkCore;
using UserApi.Domain;
using UserApi.Domain.Aggregates;
using UserApi.Domain.Repositories;

namespace UserApi.Infrastructure.Repositories;

public class UserRepository(UserDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Users.FindAsync([id], cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AnyAsync(u => u.Email == email.ToLowerInvariant(), cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync(cancellationToken);
    }
}
