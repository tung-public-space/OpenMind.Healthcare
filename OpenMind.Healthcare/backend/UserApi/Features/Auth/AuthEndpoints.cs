using MediatR;
using UserApi.Features.Auth.Commands;
using UserApi.Features.Auth.DTOs;
using UserApi.Features.Auth.Queries;

namespace UserApi.Features.Auth;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/register", Register)
            .WithName("Register")
            .WithOpenApi();

        group.MapPost("/login", Login)
            .WithName("Login")
            .WithOpenApi();

        group.MapGet("/me", GetCurrentUser)
            .WithName("GetCurrentUser")
            .RequireAuthorization()
            .WithOpenApi();

        group.MapPut("/profile", UpdateProfile)
            .WithName("UpdateProfile")
            .RequireAuthorization()
            .WithOpenApi();

        group.MapPost("/change-password", ChangePassword)
            .WithName("ChangePassword")
            .RequireAuthorization()
            .WithOpenApi();

        group.MapPost("/refresh", RefreshToken)
            .WithName("RefreshToken")
            .WithOpenApi();

        group.MapPost("/revoke", RevokeToken)
            .WithName("RevokeToken")
            .WithOpenApi();
    }

    private static async Task<IResult> Register(RegisterRequest request, HttpContext httpContext, IMediator mediator)
    {
        try
        {
            var ipAddress = GetIpAddress(httpContext);
            var command = new RegisterUserCommand(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName,
                ipAddress
            );

            var result = await mediator.Send(command);
            return Results.Created($"/api/auth/me", result);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }

    private static async Task<IResult> Login(LoginRequest request, HttpContext httpContext, IMediator mediator)
    {
        try
        {
            var ipAddress = GetIpAddress(httpContext);
            var command = new LoginUserCommand(request.Email, request.Password, ipAddress);
            var result = await mediator.Send(command);
            return Results.Ok(result);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Results.Unauthorized();
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }

    private static async Task<IResult> GetCurrentUser(HttpContext httpContext, IMediator mediator)
    {
        var userId = GetUserId(httpContext);
        if (userId == null) return Results.Unauthorized();

        var query = new GetCurrentUserQuery(userId.Value);
        var result = await mediator.Send(query);

        return result == null ? Results.NotFound() : Results.Ok(result);
    }

    private static async Task<IResult> UpdateProfile(
        UpdateProfileRequest request,
        HttpContext httpContext,
        IMediator mediator)
    {
        var userId = GetUserId(httpContext);
        if (userId == null) return Results.Unauthorized();

        try
        {
            var command = new UpdateProfileCommand(userId.Value, request.FirstName, request.LastName);
            var result = await mediator.Send(command);
            return Results.Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return Results.NotFound();
        }
    }

    private static async Task<IResult> ChangePassword(
        ChangePasswordRequest request,
        HttpContext httpContext,
        IMediator mediator)
    {
        var userId = GetUserId(httpContext);
        if (userId == null) return Results.Unauthorized();

        try
        {
            var command = new ChangePasswordCommand(userId.Value, request.CurrentPassword, request.NewPassword);
            await mediator.Send(command);
            return Results.Ok(new { message = "Password changed successfully" });
        }
        catch (KeyNotFoundException)
        {
            return Results.NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }

    private static Guid? GetUserId(HttpContext httpContext)
    {
        var userIdClaim = httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    private static string? GetIpAddress(HttpContext httpContext)
    {
        if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            return httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        }
        return httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
    }

    private static async Task<IResult> RefreshToken(RefreshTokenRequest request, HttpContext httpContext, IMediator mediator)
    {
        try
        {
            var ipAddress = GetIpAddress(httpContext);
            var command = new RefreshTokenCommand(request.RefreshToken, ipAddress);
            var result = await mediator.Send(command);
            return Results.Ok(result);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Results.Unauthorized();
        }
    }

    private static async Task<IResult> RevokeToken(RevokeTokenRequest request, HttpContext httpContext, IMediator mediator)
    {
        try
        {
            var ipAddress = GetIpAddress(httpContext);
            var command = new RevokeTokenCommand(request.RefreshToken, ipAddress);
            var result = await mediator.Send(command);

            if (!result)
            {
                return Results.BadRequest(new { message = "Token not found or already revoked" });
            }

            return Results.Ok(new { message = "Token revoked successfully" });
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }
}
