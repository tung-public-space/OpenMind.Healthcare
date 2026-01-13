using MediatR;
using UserApi.Domain.Repositories;

namespace UserApi.Features.Auth.Commands;

public record RevokeTokenCommand(string RefreshToken, string? IpAddress) : IRequest<bool>;

public class RevokeTokenHandler(
    IRefreshTokenRepository refreshTokenRepository) : IRequestHandler<RevokeTokenCommand, bool>
{
    public async Task<bool> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            throw new ArgumentException("Refresh token is required");
        }

        var storedToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);

        if (storedToken == null || !storedToken.IsActive)
        {
            return false;
        }

        storedToken.Revoke(request.IpAddress);
        await refreshTokenRepository.UpdateAsync(storedToken, cancellationToken);

        return true;
    }
}
