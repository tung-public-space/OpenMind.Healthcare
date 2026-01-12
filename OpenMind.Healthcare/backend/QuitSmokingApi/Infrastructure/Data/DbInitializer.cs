using QuitSmokingApi.Domain.Aggregates;

namespace QuitSmokingApi.Infrastructure.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        if (context.MotivationalQuotes.Any())
            return;

        SeedMotivationalQuotes(context);
        SeedCravingTips(context);
        SeedAchievements(context);
        SeedHealthMilestones(context);

        context.SaveChanges();
    }
    
    private static void SeedMotivationalQuotes(AppDbContext context)
    {
        var quotes = new[]
        {
            MotivationalQuote.Create("Every cigarette you don't smoke is a victory.", "Unknown", QuoteCategory.Encouragement),
            MotivationalQuote.Create("The secret of getting ahead is getting started.", "Mark Twain", QuoteCategory.Motivation),
            MotivationalQuote.Create("You are stronger than your cravings.", "Unknown", QuoteCategory.Strength),
            MotivationalQuote.Create("Believe you can and you're halfway there.", "Theodore Roosevelt", QuoteCategory.Belief),
            MotivationalQuote.Create("The best time to quit was yesterday. The next best time is now.", "Unknown", QuoteCategory.Action),
            MotivationalQuote.Create("Your lungs will thank you. Your heart will thank you. Your wallet will thank you.", "Unknown", QuoteCategory.Benefits),
            MotivationalQuote.Create("One day at a time. One craving at a time. You've got this!", "Unknown", QuoteCategory.Encouragement),
            MotivationalQuote.Create("Quitting smoking is the single most important thing you can do for your health.", "World Health Organization", QuoteCategory.Health),
            MotivationalQuote.Create("The pain of discipline is nothing like the pain of disappointment.", "Justin Langer", QuoteCategory.Discipline),
            MotivationalQuote.Create("You don't have to be great to start, but you have to start to be great.", "Zig Ziglar", QuoteCategory.Beginning),
            MotivationalQuote.Create("Every moment is a fresh beginning.", "T.S. Eliot", QuoteCategory.FreshStart),
            MotivationalQuote.Create("Your future self will thank you for quitting today.", "Unknown", QuoteCategory.Future),
            MotivationalQuote.Create("Cravings are temporary, but the benefits of quitting last a lifetime.", "Unknown", QuoteCategory.Cravings),
            MotivationalQuote.Create("You're not giving up anything. You're gaining everything.", "Unknown", QuoteCategory.Perspective),
            MotivationalQuote.Create("The only way to do great things is to love what you're becoming.", "Unknown", QuoteCategory.Growth)
        };
        context.MotivationalQuotes.AddRange(quotes);
    }
    
    private static void SeedCravingTips(AppDbContext context)
    {
        var tips = new[]
        {
            CravingTip.Create("Deep Breathing", "Take 10 deep breaths. Inhale for 4 seconds, hold for 4 seconds, exhale for 4 seconds.", "🌬️", TipCategory.Relaxation),
            CravingTip.Create("Drink Water", "Drink a full glass of cold water. It helps reduce cravings and keeps you hydrated.", "💧", TipCategory.Physical),
            CravingTip.Create("Take a Walk", "A 5-minute walk can reduce cravings and boost your mood with endorphins.", "🚶", TipCategory.Exercise),
            CravingTip.Create("Chew Gum", "Sugar-free gum can help satisfy the oral fixation and keep your mouth busy.", "🍬", TipCategory.Substitute),
            CravingTip.Create("Call a Friend", "Talking to someone supportive can distract you and provide encouragement.", "📞", TipCategory.Social),
            CravingTip.Create("Healthy Snack", "Munch on carrots, celery, or nuts. Keeps your hands and mouth occupied.", "🥕", TipCategory.Physical),
            CravingTip.Create("Wait 10 Minutes", "Most cravings pass within 10 minutes. Distract yourself until it passes.", "⏰", TipCategory.Mental),
            CravingTip.Create("Remember Your Why", "Think about why you quit. Your health, family, money, or freedom.", "💭", TipCategory.Mental),
            CravingTip.Create("Squeeze a Stress Ball", "Physical activity with your hands can redirect the urge.", "🎾", TipCategory.Physical),
            CravingTip.Create("Practice Mindfulness", "Acknowledge the craving without acting on it. Observe it like a passing cloud.", "🧘", TipCategory.Mental),
            CravingTip.Create("Brush Your Teeth", "The fresh, clean feeling can help reduce the desire to smoke.", "🪥", TipCategory.Physical),
            CravingTip.Create("Look at Your Progress", "Check how far you've come. Don't let one craving erase your achievements.", "📊", TipCategory.Mental)
        };
        context.CravingTips.AddRange(tips);
    }
    
    private static void SeedAchievements(AppDbContext context)
    {
        var achievements = new[]
        {
            Achievement.Create("First Step", "Started your quit smoking journey", "🌟", 0, AchievementCategory.Milestone),
            Achievement.Create("24 Hours Strong", "Completed your first day smoke-free", "🏅", 1, AchievementCategory.Milestone),
            Achievement.Create("Weekend Warrior", "Smoke-free for 3 days", "💪", 3, AchievementCategory.Milestone),
            Achievement.Create("One Week Wonder", "Completed one full week", "🎯", 7, AchievementCategory.Milestone),
            Achievement.Create("Two Week Champion", "Two weeks of freedom!", "🏆", 14, AchievementCategory.Milestone),
            Achievement.Create("Three Week Hero", "21 days - a habit is forming!", "⭐", 21, AchievementCategory.Milestone),
            Achievement.Create("Monthly Master", "One full month smoke-free!", "👑", 30, AchievementCategory.Milestone),
            Achievement.Create("Six Week Superstar", "42 days of determination", "🌈", 42, AchievementCategory.Milestone),
            Achievement.Create("Two Month Titan", "60 days of pure willpower", "🔥", 60, AchievementCategory.Milestone),
            Achievement.Create("Quarter Year Champion", "90 days - you're unstoppable!", "💎", 90, AchievementCategory.Milestone),
            Achievement.Create("Half Year Hero", "180 days of freedom", "🎊", 180, AchievementCategory.Milestone),
            Achievement.Create("Year One Legend", "365 days smoke-free!", "🏰", 365, AchievementCategory.Milestone)
        };
        context.Achievements.AddRange(achievements);
    }
    
    private static void SeedHealthMilestones(AppDbContext context)
    {
        var milestones = new[]
        {
            HealthMilestone.Create("Blood Pressure Normalizes", "Your blood pressure and pulse rate begin to return to normal.", 20, "20 minutes", "❤️", HealthCategory.Cardiovascular),
            HealthMilestone.Create("Carbon Monoxide Levels Drop", "Carbon monoxide level in your blood drops to normal.", 480, "8 hours", "🫁", HealthCategory.Respiratory),
            HealthMilestone.Create("Heart Attack Risk Decreases", "Your risk of heart attack begins to decrease.", 1440, "24 hours", "💗", HealthCategory.Cardiovascular),
            HealthMilestone.Create("Nerve Endings Regenerate", "Nerve endings start to regenerate. Taste and smell improve.", 2880, "48 hours", "👃", HealthCategory.Sensory),
            HealthMilestone.Create("Breathing Improves", "Bronchial tubes relax, making breathing easier. Lung capacity increases.", 4320, "72 hours", "🌬️", HealthCategory.Respiratory),
            HealthMilestone.Create("Circulation Improves", "Blood circulation improves significantly. Walking becomes easier.", 20160, "2 weeks", "🩸", HealthCategory.Cardiovascular),
            HealthMilestone.Create("Lung Function Increases", "Lung function increases up to 30%. Coughing and shortness of breath decrease.", 43200, "1 month", "💨", HealthCategory.Respiratory),
            HealthMilestone.Create("Cilia Regenerate", "Cilia in lungs regenerate, improving ability to clean lungs and reduce infection.", 129600, "3 months", "🧹", HealthCategory.Respiratory),
            HealthMilestone.Create("Energy Levels Increase", "Overall energy increases. Physical activity becomes much easier.", 259200, "6 months", "⚡", HealthCategory.Energy),
            HealthMilestone.Create("Coronary Heart Disease Risk Halved", "Your risk of coronary heart disease is now half that of a smoker.", 525600, "1 year", "🫀", HealthCategory.Cardiovascular),
            HealthMilestone.Create("Stroke Risk Reduced", "Your stroke risk is reduced to that of a non-smoker.", 2628000, "5 years", "🧠", HealthCategory.Cardiovascular),
            HealthMilestone.Create("Lung Cancer Risk Halved", "Risk of lung cancer drops to half that of a smoker.", 5256000, "10 years", "🎗️", HealthCategory.CancerPrevention),
            HealthMilestone.Create("Heart Disease Risk Normal", "Risk of coronary heart disease same as non-smoker.", 7884000, "15 years", "💖", HealthCategory.Cardiovascular)
        };
        context.HealthMilestones.AddRange(milestones);
    }
}
