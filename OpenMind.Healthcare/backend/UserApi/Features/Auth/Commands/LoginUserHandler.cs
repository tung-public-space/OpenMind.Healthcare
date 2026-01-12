using MediatR;
using UserApi.Domain.Repositories;
using UserApi.Features.Auth.DTOs;
using UserApi.Services;

namespace UserApi.Features.Auth.Commands;

public record LoginUserCommand(string Email, string Password) : IRequest<AuthResponse>;

public class LoginUserHandler(
    IUserRepository userRepository,
    ITokenService tokenService) : IRequestHandler<LoginUserCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("Email and password are required");
        }

        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        if (!user.IsActive)
        {
            throw new InvalidOperationException("Account is deactivated");
        }

        user.UpdateLastLogin();

        await userRepository.UpdateAsync(user, cancellationToken);

        var token = tokenService.GenerateToken(user);

        return new AuthResponse(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            token
        );
    }
}
