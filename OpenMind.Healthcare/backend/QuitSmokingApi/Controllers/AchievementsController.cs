using Microsoft.AspNetCore.Mvc;
using QuitSmokingApi.Models;
using QuitSmokingApi.Services;

namespace QuitSmokingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AchievementsController : ControllerBase
{
    private readonly IAchievementService _achievementService;

    public AchievementsController(IAchievementService achievementService)
    {
        _achievementService = achievementService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Achievement>>> GetAllAchievements()
    {
        var achievements = await _achievementService.GetAllAchievementsAsync();
        return Ok(achievements);
    }

    [HttpGet("unlocked")]
    public async Task<ActionResult<List<Achievement>>> GetUnlockedAchievements()
    {
        var achievements = await _achievementService.GetUnlockedAchievementsAsync();
        return Ok(achievements);
    }

    [HttpGet("check-new")]
    public async Task<ActionResult<Achievement>> CheckForNewAchievement()
    {
        var achievement = await _achievementService.CheckForNewAchievementAsync();
        if (achievement == null)
            return NoContent();
        return Ok(achievement);
    }
}
