using MediatR;
using QuitSmokingApi.Features.Progress.Domain;
using QuitSmokingApi.Infrastructure.Data;

namespace QuitSmokingApi.Features.Progress.GetHealthMilestones;

public class GetHealthMilestonesHandler : IRequestHandler<GetHealthMilestonesQuery, List<HealthMilestone>>
{
    private readonly AppDbContext _context;

    public GetHealthMilestonesHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<HealthMilestone>> Handle(GetHealthMilestonesQuery request, CancellationToken cancellationToken)
    {
        var progress = _context.UserProgress.FirstOrDefault();
        var minutesSinceQuit = progress != null 
            ? (int)(DateTime.UtcNow - progress.QuitDate).TotalMinutes 
            : 0;

        var milestones = new List<HealthMilestone>
        {
            new() { Id = 1, Title = "Blood Pressure Normalizes", Description = "Your blood pressure and pulse rate begin to return to normal.", TimeInMinutes = 20, TimeDisplay = "20 minutes", Icon = "❤️", Category = "Cardiovascular" },
            new() { Id = 2, Title = "Carbon Monoxide Levels Drop", Description = "Carbon monoxide level in your blood drops to normal.", TimeInMinutes = 480, TimeDisplay = "8 hours", Icon = "🫁", Category = "Respiratory" },
            new() { Id = 3, Title = "Heart Attack Risk Decreases", Description = "Your risk of heart attack begins to decrease.", TimeInMinutes = 1440, TimeDisplay = "24 hours", Icon = "💗", Category = "Cardiovascular" },
            new() { Id = 4, Title = "Nerve Endings Regenerate", Description = "Nerve endings start to regenerate. Taste and smell improve.", TimeInMinutes = 2880, TimeDisplay = "48 hours", Icon = "👃", Category = "Sensory" },
            new() { Id = 5, Title = "Breathing Improves", Description = "Bronchial tubes relax, making breathing easier. Lung capacity increases.", TimeInMinutes = 4320, TimeDisplay = "72 hours", Icon = "🌬️", Category = "Respiratory" },
            new() { Id = 6, Title = "Circulation Improves", Description = "Blood circulation improves significantly. Walking becomes easier.", TimeInMinutes = 14400, TimeDisplay = "2 weeks", Icon = "🩸", Category = "Cardiovascular" },
            new() { Id = 7, Title = "Lung Function Increases", Description = "Lung function increases up to 30%. Coughing and shortness of breath decrease.", TimeInMinutes = 43200, TimeDisplay = "1 month", Icon = "💨", Category = "Respiratory" },
            new() { Id = 8, Title = "Cilia Regenerate", Description = "Cilia in lungs regenerate, improving ability to clean lungs and reduce infection.", TimeInMinutes = 129600, TimeDisplay = "3 months", Icon = "🧹", Category = "Respiratory" },
            new() { Id = 9, Title = "Energy Levels Increase", Description = "Overall energy increases. Physical activity becomes much easier.", TimeInMinutes = 259200, TimeDisplay = "6 months", Icon = "⚡", Category = "Energy" },
            new() { Id = 10, Title = "Coronary Heart Disease Risk Halved", Description = "Your risk of coronary heart disease is now half that of a smoker.", TimeInMinutes = 525600, TimeDisplay = "1 year", Icon = "🫀", Category = "Cardiovascular" },
            new() { Id = 11, Title = "Stroke Risk Reduced", Description = "Your stroke risk is reduced to that of a non-smoker.", TimeInMinutes = 2628000, TimeDisplay = "5 years", Icon = "🧠", Category = "Cardiovascular" },
            new() { Id = 12, Title = "Lung Cancer Risk Halved", Description = "Risk of lung cancer drops to half that of a smoker.", TimeInMinutes = 5256000, TimeDisplay = "10 years", Icon = "🎗️", Category = "Cancer Prevention" },
            new() { Id = 13, Title = "Heart Disease Risk Normal", Description = "Risk of coronary heart disease same as non-smoker.", TimeInMinutes = 7884000, TimeDisplay = "15 years", Icon = "💖", Category = "Cardiovascular" }
        };

        foreach (var milestone in milestones)
        {
            milestone.IsAchieved = minutesSinceQuit >= milestone.TimeInMinutes;
            milestone.ProgressPercentage = Math.Min(100, (double)minutesSinceQuit / milestone.TimeInMinutes * 100);
        }

        return Task.FromResult(milestones);
    }
}
