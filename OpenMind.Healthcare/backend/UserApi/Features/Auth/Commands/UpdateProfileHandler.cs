using MediatR;
using UserApi.Domain.Repositories;
using UserApi.Features.Auth.DTOs;

namespace UserApi.Features.Auth.Commands;

public record UpdateProfileCommand(Guid UserId, string? FirstName, string? LastName) : IRequest<UserDto>;

public class UpdateProfileHandler(IUserRepository userRepository) : IRequestHandler<UpdateProfileCommand, UserDto>
{
    public async Task<UserDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new KeyNotFoundException("User not found");

        user.UpdateProfile(request.FirstName ?? "", request.LastName ?? "");
        await userRepository.UpdateAsync(user, cancellationToken);

        return new UserDto(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.CreatedAt,
            user.LastLoginAt
        );
    }
}
