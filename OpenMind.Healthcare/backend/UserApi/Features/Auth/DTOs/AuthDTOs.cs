using System.Text.Json.Serialization;

namespace UserApi.Features.Auth.DTOs;

public record RegisterRequest(
    string Email,
    string Password,
    [property: JsonPropertyName("firstName")] string FirstName,
    [property: JsonPropertyName("lastName")] string LastName
);

public record LoginRequest(
    string Email,
    string Password
);

public record AuthResponse(
    Guid Id,
    string Email,
    [property: JsonPropertyName("firstName")] string FirstName,
    [property: JsonPropertyName("lastName")] string LastName,
    string Token
);

public record UserDto(
    Guid Id,
    string Email,
    [property: JsonPropertyName("firstName")] string FirstName,
    [property: JsonPropertyName("lastName")] string LastName,
    DateTime CreatedAt,
    DateTime? LastLoginAt
);

public record UpdateProfileRequest(
    string FirstName,
    string LastName
);

public record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword
);
