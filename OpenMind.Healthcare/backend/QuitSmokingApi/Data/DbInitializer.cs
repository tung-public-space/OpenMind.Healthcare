using QuitSmokingApi.Models;

namespace QuitSmokingApi.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        if (context.MotivationalQuotes.Any())
            return;

        // Seed Motivational Quotes
        var quotes = new List<MotivationalQuote>
        {
            new() { Quote = "Every cigarette you don't smoke is a victory.", Author = "Unknown", Category = "Encouragement" },
            new() { Quote = "The secret of getting ahead is getting started.", Author = "Mark Twain", Category = "Motivation" },
            new() { Quote = "You are stronger than your cravings.", Author = "Unknown", Category = "Strength" },
            new() { Quote = "Believe you can and you're halfway there.", Author = "Theodore Roosevelt", Category = "Belief" },
            new() { Quote = "The best time to quit was yesterday. The next best time is now.", Author = "Unknown", Category = "Action" },
            new() { Quote = "Your lungs will thank you. Your heart will thank you. Your wallet will thank you.", Author = "Unknown", Category = "Benefits" },
            new() { Quote = "One day at a time. One craving at a time. You've got this!", Author = "Unknown", Category = "Encouragement" },
            new() { Quote = "Quitting smoking is the single most important thing you can do for your health.", Author = "World Health Organization", Category = "Health" },
            new() { Quote = "The pain of discipline is nothing like the pain of disappointment.", Author = "Justin Langer", Category = "Discipline" },
            new() { Quote = "You don't have to be great to start, but you have to start to be great.", Author = "Zig Ziglar", Category = "Beginning" },
            new() { Quote = "Every moment is a fresh beginning.", Author = "T.S. Eliot", Category = "Fresh Start" },
            new() { Quote = "Your future self will thank you for quitting today.", Author = "Unknown", Category = "Future" },
            new() { Quote = "Cravings are temporary, but the benefits of quitting last a lifetime.", Author = "Unknown", Category = "Cravings" },
            new() { Quote = "You're not giving up anything. You're gaining everything.", Author = "Unknown", Category = "Perspective" },
            new() { Quote = "The only way to do great things is to love what you're becoming.", Author = "Unknown", Category = "Growth" }
        };
        context.MotivationalQuotes.AddRange(quotes);

        // Seed Craving Tips
        var tips = new List<CravingTip>
        {
            new() { Title = "Deep Breathing", Description = "Take 10 deep breaths. Inhale for 4 seconds, hold for 4 seconds, exhale for 4 seconds.", Icon = "🌬️", Category = "Relaxation" },
            new() { Title = "Drink Water", Description = "Drink a full glass of cold water. It helps reduce cravings and keeps you hydrated.", Icon = "💧", Category = "Physical" },
            new() { Title = "Take a Walk", Description = "A 5-minute walk can reduce cravings and boost your mood with endorphins.", Icon = "🚶", Category = "Exercise" },
            new() { Title = "Chew Gum", Description = "Sugar-free gum can help satisfy the oral fixation and keep your mouth busy.", Icon = "🍬", Category = "Substitute" },
            new() { Title = "Call a Friend", Description = "Talking to someone supportive can distract you and provide encouragement.", Icon = "📞", Category = "Social" },
            new() { Title = "Healthy Snack", Description = "Munch on carrots, celery, or nuts. Keeps your hands and mouth occupied.", Icon = "🥕", Category = "Physical" },
            new() { Title = "Wait 10 Minutes", Description = "Most cravings pass within 10 minutes. Distract yourself until it passes.", Icon = "⏰", Category = "Mental" },
            new() { Title = "Remember Your Why", Description = "Think about why you quit. Your health, family, money, or freedom.", Icon = "💭", Category = "Mental" },
            new() { Title = "Squeeze a Stress Ball", Description = "Physical activity with your hands can redirect the urge.", Icon = "🎾", Category = "Physical" },
            new() { Title = "Practice Mindfulness", Description = "Acknowledge the craving without acting on it. Observe it like a passing cloud.", Icon = "🧘", Category = "Mental" },
            new() { Title = "Brush Your Teeth", Description = "The fresh, clean feeling can help reduce the desire to smoke.", Icon = "🪥", Category = "Physical" },
            new() { Title = "Look at Your Progress", Description = "Check how far you've come. Don't let one craving erase your achievements.", Icon = "📊", Category = "Mental" }
        };
        context.CravingTips.AddRange(tips);

        // Seed Achievements
        var achievements = new List<Achievement>
        {
            new() { Name = "First Step", Description = "Started your quit smoking journey", Icon = "🌟", RequiredDays = 0, Category = "Milestone" },
            new() { Name = "24 Hours Strong", Description = "Completed your first day smoke-free", Icon = "🏅", RequiredDays = 1, Category = "Milestone" },
            new() { Name = "Weekend Warrior", Description = "Smoke-free for 3 days", Icon = "💪", RequiredDays = 3, Category = "Milestone" },
            new() { Name = "One Week Wonder", Description = "Completed one full week", Icon = "🎯", RequiredDays = 7, Category = "Milestone" },
            new() { Name = "Two Week Champion", Description = "Two weeks of freedom!", Icon = "🏆", RequiredDays = 14, Category = "Milestone" },
            new() { Name = "Three Week Hero", Description = "21 days - a habit is forming!", Icon = "⭐", RequiredDays = 21, Category = "Milestone" },
            new() { Name = "Monthly Master", Description = "One full month smoke-free!", Icon = "👑", RequiredDays = 30, Category = "Milestone" },
            new() { Name = "Six Week Superstar", Description = "42 days of determination", Icon = "🌈", RequiredDays = 42, Category = "Milestone" },
            new() { Name = "Two Month Titan", Description = "60 days of pure willpower", Icon = "🔥", RequiredDays = 60, Category = "Milestone" },
            new() { Name = "Quarter Year Champion", Description = "90 days - you're unstoppable!", Icon = "💎", RequiredDays = 90, Category = "Milestone" },
            new() { Name = "Half Year Hero", Description = "180 days of freedom", Icon = "🎊", RequiredDays = 180, Category = "Milestone" },
            new() { Name = "Year One Legend", Description = "365 days smoke-free!", Icon = "🏰", RequiredDays = 365, Category = "Milestone" }
        };
        context.Achievements.AddRange(achievements);

        context.SaveChanges();
    }
}
