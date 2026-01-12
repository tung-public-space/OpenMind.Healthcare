using MediatR;
using UserApi.Domain.Repositories;

namespace UserApi.Features.Auth.Commands;

public record ChangePasswordCommand(Guid UserId, string CurrentPassword, string NewPassword) : IRequest<Unit>;

public class ChangePasswordHandler(IUserRepository userRepository) : IRequestHandler<ChangePasswordCommand, Unit>
{
    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new KeyNotFoundException("User not found");

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
        {
            throw new InvalidOperationException("Current password is incorrect");
        }

        if (request.NewPassword.Length < 6)
        {
            throw new ArgumentException("New password must be at least 6 characters");
        }

        var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.ChangePassword(newPasswordHash);
        await userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}
