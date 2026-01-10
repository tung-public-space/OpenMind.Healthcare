using MediatR;
using QuitSmokingApi.Features.Motivation.Domain;
using QuitSmokingApi.Features.Motivation.GetRandomQuote;
using QuitSmokingApi.Features.Progress.Domain;
using QuitSmokingApi.Features.Progress.GetStats;
using QuitSmokingApi.Infrastructure.Data;

namespace QuitSmokingApi.Features.Motivation.GetDailyEncouragement;

public class GetDailyEncouragementHandler : IRequestHandler<GetDailyEncouragementQuery, DailyEncouragement>
{
    private readonly AppDbContext _context;
    private readonly IMediator _mediator;

    public GetDailyEncouragementHandler(AppDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<DailyEncouragement> Handle(GetDailyEncouragementQuery request, CancellationToken cancellationToken)
    {
        var stats = await _mediator.Send(new GetStatsQuery(), cancellationToken);
        var quote = await _mediator.Send(new GetRandomQuoteQuery(), cancellationToken);
        var tips = _context.CravingTips.ToList();
        var random = new Random();
        
        // Get 3 random tips
        var randomTips = tips.OrderBy(x => random.Next()).Take(3).ToList();

        var encouragement = new DailyEncouragement
        {
            Message = GenerateEncouragementMessage(stats),
            Quote = quote,
            Tips = randomTips,
            SpecialMessage = GenerateSpecialMessage(stats)
        };

        return encouragement;
    }

    private static string GenerateEncouragementMessage(ProgressStats stats)
    {
        if (stats.DaysSmokeFree == 0)
        {
            return "Today is Day 1! Every journey begins with a single step. You've made the most important decision of your life. Stay strong! 💪";
        }
        else if (stats.DaysSmokeFree == 1)
        {
            return "Congratulations on completing your first day! The first 24 hours are the hardest. You're already proving you can do this! 🎉";
        }
        else if (stats.DaysSmokeFree < 7)
        {
            return $"Amazing! {stats.DaysSmokeFree} days smoke-free! You've already avoided {stats.CigarettesNotSmoked} cigarettes and saved ${stats.MoneySaved}. Keep pushing through! 💪";
        }
        else if (stats.DaysSmokeFree < 14)
        {
            return $"Week one complete! You're in week {stats.DaysSmokeFree / 7 + 1} now. Your body is already healing. You've saved ${stats.MoneySaved} and gained {stats.LifeRegainedFormatted} of life! 🌟";
        }
        else if (stats.DaysSmokeFree < 30)
        {
            return $"Incredible progress! {stats.DaysSmokeFree} days strong! You're proving that you're stronger than any craving. ${stats.MoneySaved} saved and counting! 🏆";
        }
        else if (stats.DaysSmokeFree < 90)
        {
            return $"Over a month smoke-free! {stats.DaysSmokeFree} days of freedom. Your lungs are thanking you every breath. You've saved ${stats.MoneySaved}! 🎊";
        }
        else
        {
            return $"Legendary status! {stats.DaysSmokeFree} days smoke-free! You've transformed your life. ${stats.MoneySaved} saved, {stats.CigarettesNotSmoked} cigarettes avoided, {stats.LifeRegainedFormatted} of life regained! 👑";
        }
    }

    private static string GenerateSpecialMessage(ProgressStats stats)
    {
        if (stats.DaysSmokeFree == 7)
            return "🎉 MILESTONE: ONE WEEK! Your taste and smell are improving!";
        if (stats.DaysSmokeFree == 14)
            return "🎉 MILESTONE: TWO WEEKS! Your circulation is significantly better!";
        if (stats.DaysSmokeFree == 21)
            return "🎉 MILESTONE: THREE WEEKS! A new habit is forming - a smoke-free you!";
        if (stats.DaysSmokeFree == 30)
            return "🎉 MILESTONE: ONE MONTH! Your lung function is increasing!";
        if (stats.DaysSmokeFree == 90)
            return "🎉 MILESTONE: THREE MONTHS! Your risk of heart attack has decreased significantly!";
        if (stats.DaysSmokeFree == 180)
            return "🎉 MILESTONE: SIX MONTHS! Your energy levels have dramatically increased!";
        if (stats.DaysSmokeFree == 365)
            return "🎉 MILESTONE: ONE YEAR! Your risk of heart disease is now HALF that of a smoker!";
        
        return string.Empty;
    }
}
