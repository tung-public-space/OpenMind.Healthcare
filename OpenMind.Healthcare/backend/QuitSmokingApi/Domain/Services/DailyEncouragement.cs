namespace QuitSmokingApi.Domain.Services;

/// <summary>
/// Value object representing daily encouragement content
/// </summary>
public record DailyEncouragement(
    string Message,
    string? SpecialMessage,
    string CravingEncouragement
);
