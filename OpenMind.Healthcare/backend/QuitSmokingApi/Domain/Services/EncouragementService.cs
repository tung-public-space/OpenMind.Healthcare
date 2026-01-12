using QuitSmokingApi.Domain.ValueObjects;

namespace QuitSmokingApi.Domain.Services;

public class EncouragementService
{
    public string GenerateEncouragementMessage(ProgressStatistics stats)
    {
        return stats.DaysSmokeFree switch
        {
            0 => "Today is Day 1! Every journey begins with a single step. You've made the most important decision of your life. Stay strong! 💪",
            1 => "Congratulations on completing your first day! The first 24 hours are the hardest. You're already proving you can do this! 🎉",
            < 7 => $"Amazing! {stats.DaysSmokeFree} days smoke-free! You've already avoided {stats.CigarettesAvoided} cigarettes and saved ${stats.MoneySaved.Amount}. Keep pushing through! 💪",
            < 14 => $"Week one complete! You're in week {stats.DaysSmokeFree / 7 + 1} now. Your body is already healing. You've saved ${stats.MoneySaved.Amount} and gained {stats.LifeRegained.ToFriendlyString()} of life! 🌟",
            < 30 => $"Incredible progress! {stats.DaysSmokeFree} days strong! You're proving that you're stronger than any craving. ${stats.MoneySaved.Amount} saved and counting! 🏆",
            < 90 => $"Over a month smoke-free! {stats.DaysSmokeFree} days of freedom. Your lungs are thanking you every breath. You've saved ${stats.MoneySaved.Amount}! 🎊",
            _ => $"Legendary status! {stats.DaysSmokeFree} days smoke-free! You've transformed your life. ${stats.MoneySaved.Amount} saved, {stats.CigarettesAvoided} cigarettes avoided, {stats.LifeRegained.ToFriendlyString()} of life regained! 👑"
        };
    }
    
    public string? GenerateSpecialMilestoneMessage(int daysSmokeFree)
    {
        return daysSmokeFree switch
        {
            7 => "🎉 MILESTONE: ONE WEEK! Your taste and smell are improving!",
            14 => "🎉 MILESTONE: TWO WEEKS! Your circulation is significantly better!",
            21 => "🎉 MILESTONE: THREE WEEKS! A new habit is forming - a smoke-free you!",
            30 => "🎉 MILESTONE: ONE MONTH! Your lung function is increasing!",
            90 => "🎉 MILESTONE: THREE MONTHS! Your risk of heart attack has decreased significantly!",
            180 => "🎉 MILESTONE: SIX MONTHS! Your energy levels have dramatically increased!",
            365 => "🎉 MILESTONE: ONE YEAR! Your risk of heart disease is now HALF that of a smoker!",
            _ => null
        };
    }
    
    public string GenerateCravingEncouragement(int daysSmokeFree)
    {
        if (daysSmokeFree < 3)
            return "Cravings are intense right now, but they typically last only 3-5 minutes. You can do this! 💪";
            
        if (daysSmokeFree < 7)
            return "You're past the hardest physical withdrawal phase. Cravings will become less frequent! 🌟";
            
        if (daysSmokeFree < 14)
            return "Your brain is rewiring itself. Each craving you overcome makes you stronger! 🧠";
            
        if (daysSmokeFree < 30)
            return "You've built a solid foundation. Cravings are now more habit than addiction. Stay vigilant! 🏗️";
            
        return "You're a craving-crushing champion! Remember how far you've come. You don't need cigarettes anymore! 🏆";
    }
}
