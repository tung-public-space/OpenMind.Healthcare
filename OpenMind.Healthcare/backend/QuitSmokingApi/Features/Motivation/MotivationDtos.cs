namespace QuitSmokingApi.Features.Motivation;

/// <summary>
/// DTO for motivational quote response
/// </summary>
public record MotivationalQuoteDto(
    Guid Id,
    string Quote,
    string Author,
    string Category
);

/// <summary>
/// DTO for craving tip response
/// </summary>
public record CravingTipDto(
    Guid Id,
    string Title,
    string Description,
    string Icon,
    string Category
);

/// <summary>
/// DTO for daily encouragement response
/// </summary>
public record DailyEncouragementDto(
    string Message,
    string? SpecialMessage,
    MotivationalQuoteDto Quote,
    List<CravingTipDto> Tips,
    string CravingEncouragement
);
