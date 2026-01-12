using MediatR;
using UserApi.Domain.Repositories;
using UserApi.Features.Auth.DTOs;

namespace UserApi.Features.Auth.Queries;

public record GetCurrentUserQuery(Guid UserId) : IRequest<UserDto?>;

public class GetCurrentUserHandler(IUserRepository userRepository) : IRequestHandler<GetCurrentUserQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        
        if (user == null)
            return null;

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
