using Microsoft.EntityFrameworkCore;
using UserApi.Domain;
using UserApi.Features.Auth.DTOs;
using UserApi.Infrastructure;
using UserApi.Services;

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
    }

    private static async Task<IResult> Register(
        RegisterRequest request,
        UserDbContext db,
        ITokenService tokenService)
    {
        // Validate request
        if (string.IsNullOrWhiteSpace(request.Email) || 
            string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return Results.BadRequest(new { message = "Email, username, and password are required" });
        }

        if (request.Password.Length < 6)
        {
            return Results.BadRequest(new { message = "Password must be at least 6 characters" });
        }

        // Check if email already exists
        var existingEmail = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Email.ToLowerInvariant());
        if (existingEmail != null)
        {
            return Results.BadRequest(new { message = "Email already registered" });
        }

        // Check if username already exists
        var existingUsername = await db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (existingUsername != null)
        {
            return Results.BadRequest(new { message = "Username already taken" });
        }

        // Hash password and create user
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = User.Create(
            request.Email,
            request.Username,
            passwordHash,
            request.FirstName ?? "",
            request.LastName ?? ""
        );

        db.Users.Add(user);
        await db.SaveChangesAsync();

        // Generate token
        var token = tokenService.GenerateToken(user);

        return Results.Created($"/api/auth/me", new AuthResponse(
            user.Id,
            user.Email,
            user.Username,
            user.FirstName,
            user.LastName,
            token
        ));
    }

    private static async Task<IResult> Login(
        LoginRequest request,
        UserDbContext db,
        ITokenService tokenService)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return Results.BadRequest(new { message = "Email and password are required" });
        }

        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Email.ToLowerInvariant());
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Results.Unauthorized();
        }

        if (!user.IsActive)
        {
            return Results.BadRequest(new { message = "Account is deactivated" });
        }

        user.UpdateLastLogin();
        await db.SaveChangesAsync();

        var token = tokenService.GenerateToken(user);

        return Results.Ok(new AuthResponse(
            user.Id,
            user.Email,
            user.Username,
            user.FirstName,
            user.LastName,
            token
        ));
    }

    private static async Task<IResult> GetCurrentUser(
        HttpContext httpContext,
        UserDbContext db)
    {
        var userId = GetUserId(httpContext);
        if (userId == null) return Results.Unauthorized();

        var user = await db.Users.FindAsync(userId.Value);
        if (user == null) return Results.NotFound();

        return Results.Ok(new UserDto(
            user.Id,
            user.Email,
            user.Username,
            user.FirstName,
            user.LastName,
            user.CreatedAt,
            user.LastLoginAt
        ));
    }

    private static async Task<IResult> UpdateProfile(
        UpdateProfileRequest request,
        HttpContext httpContext,
        UserDbContext db)
    {
        var userId = GetUserId(httpContext);
        if (userId == null) return Results.Unauthorized();

        var user = await db.Users.FindAsync(userId.Value);
        if (user == null) return Results.NotFound();

        user.UpdateProfile(request.FirstName ?? "", request.LastName ?? "");
        await db.SaveChangesAsync();

        return Results.Ok(new UserDto(
            user.Id,
            user.Email,
            user.Username,
            user.FirstName,
            user.LastName,
            user.CreatedAt,
            user.LastLoginAt
        ));
    }

    private static async Task<IResult> ChangePassword(
        ChangePasswordRequest request,
        HttpContext httpContext,
        UserDbContext db)
    {
        var userId = GetUserId(httpContext);
        if (userId == null) return Results.Unauthorized();

        var user = await db.Users.FindAsync(userId.Value);
        if (user == null) return Results.NotFound();

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
        {
            return Results.BadRequest(new { message = "Current password is incorrect" });
        }

        if (request.NewPassword.Length < 6)
        {
            return Results.BadRequest(new { message = "New password must be at least 6 characters" });
        }

        var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.ChangePassword(newPasswordHash);
        await db.SaveChangesAsync();

        return Results.Ok(new { message = "Password changed successfully" });
    }

    private static Guid? GetUserId(HttpContext httpContext)
    {
        var userIdClaim = httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}
