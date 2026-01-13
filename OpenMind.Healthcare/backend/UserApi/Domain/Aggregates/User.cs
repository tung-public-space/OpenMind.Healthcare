using DDD.BuildingBlocks;
using UserApi.Domain.Events;

namespace UserApi.Domain.Aggregates;

public class User : AggregateRoot
{
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public DateTime? LastLoginAt { get; private set; }
    public bool IsActive { get; private set; }

    private User() { }

    private User(string email, string passwordHash, string firstName, string lastName)
    {
        Email = email.ToLowerInvariant();
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;
        IsActive = true;
    }

    public static User Create(string email, string passwordHash, string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email is required");
            
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash is required");

        var user = new User(email, passwordHash, firstName, lastName);
        user.Emit(new UserRegisteredEvent(user.Id, user.Email));
        return user;
    }

    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        SetUpdated();
        Emit(new UserLoggedInEvent(Id, LastLoginAt.Value));
    }

    public void UpdateProfile(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        SetUpdated();
        Emit(new ProfileUpdatedEvent(Id, firstName, lastName));
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new DomainException("Password hash is required");
            
        PasswordHash = newPasswordHash;
        SetUpdated();
        Emit(new PasswordChangedEvent(Id));
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdated();
    }

    public void Activate()
    {
        IsActive = true;
        SetUpdated();
    }
}
