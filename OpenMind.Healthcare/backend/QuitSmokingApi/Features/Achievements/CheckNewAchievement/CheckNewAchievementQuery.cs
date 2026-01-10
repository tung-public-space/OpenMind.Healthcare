using MediatR;

namespace QuitSmokingApi.Features.Achievements.CheckNewAchievement;

public record CheckNewAchievementQuery : IRequest<AchievementDto?>;
