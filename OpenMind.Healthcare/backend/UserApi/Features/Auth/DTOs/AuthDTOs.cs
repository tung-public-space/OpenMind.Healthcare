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
    [property: JsonPropertyName("accessToken")] string AccessToken,
    [property: JsonPropertyName("refreshToken")] string RefreshToken
);

public record RefreshTokenRequest(
    [property: JsonPropertyName("refreshToken")] string RefreshToken
);

public record RevokeTokenRequest(
    [property: JsonPropertyName("refreshToken")] string RefreshToken
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
