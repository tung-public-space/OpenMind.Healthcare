using MediatR;
using UserApi.Domain;
using UserApi.Domain.Aggregates;
using UserApi.Domain.Repositories;
using UserApi.Features.Auth.DTOs;
using UserApi.Services;

namespace UserApi.Features.Auth.Commands;

public record RegisterUserCommand(
    string Email,
    string Password,
    string? FirstName,
    string? LastName,
    string? IpAddress) : IRequest<AuthResponse>;

public class RegisterUserHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    ITokenService tokenService) : IRequestHandler<RegisterUserCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("Email and password are required");
        }

        if (request.Password.Length < 6)
        {
            throw new ArgumentException("Password must be at least 6 characters");
        }

        if (await userRepository.EmailExistsAsync(request.Email, cancellationToken))
        {
            throw new InvalidOperationException("Email already registered");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = User.Create(
            request.Email,
            passwordHash,
            request.FirstName ?? "",
            request.LastName ?? ""
        );

        await userRepository.AddAsync(user, cancellationToken);

        var accessToken = tokenService.GenerateAccessToken(user);
        var refreshToken = tokenService.GenerateRefreshToken(user.Id, request.IpAddress ?? "unknown");

        await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        return new AuthResponse(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            accessToken,
            refreshToken.Token
        );
    }
}
