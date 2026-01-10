namespace QuitSmokingApi.Features.Achievements;

/// <summary>
/// DTO for achievement response - maps from domain entity
/// </summary>
public record AchievementDto(
    Guid Id,
    string Name,
    string Description,
    string Icon,
    int RequiredDays,
    string Category,
    bool IsUnlocked,
    DateTime? UnlockedAt,
    double ProgressPercentage
);
