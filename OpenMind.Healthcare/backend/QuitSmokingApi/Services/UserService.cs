using System.Security.Claims;

namespace QuitSmokingApi.Services;

public interface IUserService
{
    Guid? GetCurrentUserId();
    string? GetCurrentUserEmail();
}

public class UserService(IHttpContextAccessor httpContextAccessor) : IUserService
{
    public Guid? GetCurrentUserId()
    {
        var userIdClaim = httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    public string? GetCurrentUserEmail()
    {
        return httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.Email)?.Value;
    }
}