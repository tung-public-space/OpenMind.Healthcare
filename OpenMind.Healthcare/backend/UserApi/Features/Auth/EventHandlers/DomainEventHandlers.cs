using MediatR;
using UserApi.Domain.Events;

namespace UserApi.Features.Auth.EventHandlers;

public class UserRegisteredEventHandler(ILogger<UserRegisteredEventHandler> logger)
    : INotificationHandler<UserRegisteredEvent>
{
    public Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "User registered: {UserId}, Email: {Email}",
            notification.UserId,
            notification.Email);

        return Task.CompletedTask;
    }
}

public class UserLoggedInEventHandler(ILogger<UserLoggedInEventHandler> logger)
    : INotificationHandler<UserLoggedInEvent>
{
    public Task Handle(UserLoggedInEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "User logged in: {UserId} at {LoginTime}",
            notification.UserId,
            notification.LoginTime);

        return Task.CompletedTask;
    }
}

public class PasswordChangedEventHandler(ILogger<PasswordChangedEventHandler> logger)
    : INotificationHandler<PasswordChangedEvent>
{
    public Task Handle(PasswordChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Password changed for user: {UserId}", notification.UserId);

        return Task.CompletedTask;
    }
}

public class ProfileUpdatedEventHandler(ILogger<ProfileUpdatedEventHandler> logger)
    : INotificationHandler<ProfileUpdatedEvent>
{
    public Task Handle(ProfileUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Profile updated for user: {UserId}, Name: {FirstName} {LastName}",
            notification.UserId,
            notification.FirstName,
            notification.LastName);

        return Task.CompletedTask;
    }
}
