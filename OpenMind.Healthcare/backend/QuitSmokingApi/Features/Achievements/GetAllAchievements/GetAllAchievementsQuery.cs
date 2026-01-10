using MediatR;
using QuitSmokingApi.Features.Achievements.Domain;

namespace QuitSmokingApi.Features.Achievements.GetAllAchievements;

public record GetAllAchievementsQuery : IRequest<List<Achievement>>;
