using Microsoft.AspNetCore.Mvc;
using QuitSmokingApi.Models;
using QuitSmokingApi.Services;

namespace QuitSmokingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MotivationController : ControllerBase
{
    private readonly IMotivationService _motivationService;

    public MotivationController(IMotivationService motivationService)
    {
        _motivationService = motivationService;
    }

    [HttpGet("quote")]
    public async Task<ActionResult<MotivationalQuote>> GetRandomQuote()
    {
        var quote = await _motivationService.GetRandomQuoteAsync();
        return Ok(quote);
    }

    [HttpGet("craving-tips")]
    public async Task<ActionResult<List<CravingTip>>> GetCravingTips()
    {
        var tips = await _motivationService.GetCravingTipsAsync();
        return Ok(tips);
    }

    [HttpGet("daily")]
    public async Task<ActionResult<DailyEncouragement>> GetDailyEncouragement()
    {
        var encouragement = await _motivationService.GetDailyEncouragementAsync();
        return Ok(encouragement);
    }
}
