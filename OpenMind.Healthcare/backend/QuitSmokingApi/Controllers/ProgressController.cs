using Microsoft.AspNetCore.Mvc;
using QuitSmokingApi.Models;
using QuitSmokingApi.Services;

namespace QuitSmokingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProgressController : ControllerBase
{
    private readonly IProgressService _progressService;

    public ProgressController(IProgressService progressService)
    {
        _progressService = progressService;
    }

    [HttpGet]
    public async Task<ActionResult<UserProgress>> GetProgress()
    {
        var progress = await _progressService.GetUserProgressAsync();
        if (progress == null)
            return NotFound();
        return Ok(progress);
    }

    [HttpPost]
    public async Task<ActionResult<UserProgress>> CreateOrUpdateProgress([FromBody] UserProgress progress)
    {
        var result = await _progressService.CreateOrUpdateProgressAsync(progress);
        return Ok(result);
    }

    [HttpGet("stats")]
    public async Task<ActionResult<ProgressStats>> GetStats()
    {
        var stats = await _progressService.GetProgressStatsAsync();
        return Ok(stats);
    }

    [HttpGet("health-milestones")]
    public async Task<ActionResult<List<HealthMilestone>>> GetHealthMilestones()
    {
        var milestones = await _progressService.GetHealthMilestonesAsync();
        return Ok(milestones);
    }
}
