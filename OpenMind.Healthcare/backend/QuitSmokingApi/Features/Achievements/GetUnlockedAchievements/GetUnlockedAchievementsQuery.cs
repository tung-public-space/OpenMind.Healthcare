using MediatR;
using QuitSmokingApi.Features.Achievements.Domain;

namespace QuitSmokingApi.Features.Achievements.GetUnlockedAchievements;

public record GetUnlockedAchievementsQuery : IRequest<List<Achievement>>;
