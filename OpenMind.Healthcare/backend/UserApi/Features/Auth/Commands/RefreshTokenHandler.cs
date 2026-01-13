using MediatR;
using UserApi.Domain.Repositories;
using UserApi.Features.Auth.DTOs;
using UserApi.Services;

namespace UserApi.Features.Auth.Commands;

public record RefreshTokenCommand(string RefreshToken, string? IpAddress) : IRequest<AuthResponse>;

public class RefreshTokenHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository,
    ITokenService tokenService) : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            throw new ArgumentException("Refresh token is required");
        }

        var storedToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

        if (storedToken == null)
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        if (storedToken.IsRevoked)
        {
            // Token reuse detected - revoke all tokens for security
            await refreshTokenRepository.RevokeAllUserTokensAsync(storedToken.UserId, request.IpAddress, cancellationToken);
            throw new UnauthorizedAccessException("Token has been revoked. All sessions have been terminated for security.");
        }

        if (storedToken.IsExpired)
        {
            throw new UnauthorizedAccessException("Refresh token has expired");
        }

        var user = await userRepository.GetByIdAsync(storedToken.UserId, cancellationToken);

        if (user == null || !user.IsActive)
        {
            throw new UnauthorizedAccessException("User not found or inactive");
        }

        // Rotate refresh token
        var newRefreshToken = tokenService.GenerateRefreshToken(user.Id, request.IpAddress ?? "unknown");
        storedToken.Revoke(request.IpAddress, newRefreshToken.Token);

        await refreshTokenRepository.UpdateAsync(storedToken, cancellationToken);
        await refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);

        var accessToken = tokenService.GenerateAccessToken(user);

        return new AuthResponse(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            accessToken,
            newRefreshToken.Token
        );
    }
}
