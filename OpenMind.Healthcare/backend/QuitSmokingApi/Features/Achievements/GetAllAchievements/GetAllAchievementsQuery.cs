using MediatR;

namespace QuitSmokingApi.Features.Achievements.GetAllAchievements;

public record GetAllAchievementsQuery : IRequest<List<AchievementDto>>;
