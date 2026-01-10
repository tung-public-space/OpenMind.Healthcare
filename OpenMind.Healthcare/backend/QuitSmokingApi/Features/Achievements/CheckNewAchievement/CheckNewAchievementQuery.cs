using MediatR;
using QuitSmokingApi.Features.Achievements.Domain;

namespace QuitSmokingApi.Features.Achievements.CheckNewAchievement;

public record CheckNewAchievementQuery : IRequest<Achievement?>;
