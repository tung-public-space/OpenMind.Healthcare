using QuitSmokingApi.Domain.Common;

namespace QuitSmokingApi.Domain.Aggregates;

/// <summary>
/// Entity representing a user's unlocked achievement
/// </summary>
public class UserAchievement : Entity
{
    public Guid UserId { get; private set; }
    public Guid AchievementId { get; private set; }
    public DateTime UnlockedAt { get; private set; }
    public Achievement Achievement { get; private set; } = null!;
    
    // Private constructor for EF Core
    private UserAchievement() { }
    
    private UserAchievement(Guid userId, Guid achievementId, DateTime unlockedAt)
    {
        UserId = userId;
        AchievementId = achievementId;
        UnlockedAt = unlockedAt;
    }
    
    public static UserAchievement Unlock(Guid userId, Achievement achievement)
    {
        if (achievement == null)
            throw new DomainException("Achievement cannot be null");
            
        return new UserAchievement(userId, achievement.Id, DateTime.UtcNow)
        {
            Achievement = achievement
        };
    }
}
