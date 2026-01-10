using MediatR;

namespace QuitSmokingApi.Features.Achievements.GetUnlockedAchievements;

public record GetUnlockedAchievementsQuery : IRequest<List<AchievementDto>>;
