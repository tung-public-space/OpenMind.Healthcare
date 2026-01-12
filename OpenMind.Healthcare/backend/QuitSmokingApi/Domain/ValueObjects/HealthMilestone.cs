using QuitSmokingApi.Domain.Common;

namespace QuitSmokingApi.Domain.ValueObjects;

/// <summary>
/// Value object representing a health milestone and its benefits.
/// Value objects are immutable and identified by their values, not by an ID.
/// </summary>
public class HealthMilestone : ValueObject
{
    public string Title { get; }
    public string Description { get; }
    public Duration TimeRequired { get; }
    public string TimeDisplay { get; }
    public string Icon { get; }
    public HealthCategory Category { get; }
    
    private HealthMilestone(string title, string description, Duration timeRequired, string timeDisplay, string icon, HealthCategory category)
    {
        Title = title;
        Description = description;
        TimeRequired = timeRequired;
        TimeDisplay = timeDisplay;
        Icon = icon;
        Category = category;
    }
    
    // Pre-defined health milestones based on medical research
    public static readonly HealthMilestone BloodPressureNormalizes = new(
        "Blood Pressure Normalizes",
        "Your blood pressure and pulse rate begin to return to normal.",
        Duration.FromMinutes(20),
        "20 minutes",
        "❤️",
        HealthCategory.Cardiovascular);
        
    public static readonly HealthMilestone CarbonMonoxideDrops = new(
        "Carbon Monoxide Levels Drop",
        "Carbon monoxide level in your blood drops to normal.",
        Duration.FromHours(8),
        "8 hours",
        "🫁",
        HealthCategory.Respiratory);
        
    public static readonly HealthMilestone HeartAttackRiskDecreases = new(
        "Heart Attack Risk Decreases",
        "Your risk of heart attack begins to decrease.",
        Duration.FromDays(1),
        "24 hours",
        "💗",
        HealthCategory.Cardiovascular);
        
    public static readonly HealthMilestone NerveEndingsRegenerate = new(
        "Nerve Endings Regenerate",
        "Nerve endings start to regenerate. Taste and smell improve.",
        Duration.FromDays(2),
        "48 hours",
        "👃",
        HealthCategory.Sensory);
        
    public static readonly HealthMilestone BreathingImproves = new(
        "Breathing Improves",
        "Bronchial tubes relax, making breathing easier. Lung capacity increases.",
        Duration.FromDays(3),
        "72 hours",
        "🌬️",
        HealthCategory.Respiratory);
        
    public static readonly HealthMilestone CirculationImproves = new(
        "Circulation Improves",
        "Blood circulation improves significantly. Walking becomes easier.",
        Duration.FromDays(14),
        "2 weeks",
        "🩸",
        HealthCategory.Cardiovascular);
        
    public static readonly HealthMilestone LungFunctionIncreases = new(
        "Lung Function Increases",
        "Lung function increases up to 30%. Coughing and shortness of breath decrease.",
        Duration.FromDays(30),
        "1 month",
        "💨",
        HealthCategory.Respiratory);
        
    public static readonly HealthMilestone CiliaRegenerate = new(
        "Cilia Regenerate",
        "Cilia in lungs regenerate, improving ability to clean lungs and reduce infection.",
        Duration.FromDays(90),
        "3 months",
        "🧹",
        HealthCategory.Respiratory);
        
    public static readonly HealthMilestone EnergyLevelsIncrease = new(
        "Energy Levels Increase",
        "Overall energy increases. Physical activity becomes much easier.",
        Duration.FromDays(180),
        "6 months",
        "⚡",
        HealthCategory.Energy);
        
    public static readonly HealthMilestone HeartDiseaseRiskHalved = new(
        "Coronary Heart Disease Risk Halved",
        "Your risk of coronary heart disease is now half that of a smoker.",
        Duration.FromDays(365),
        "1 year",
        "🫀",
        HealthCategory.Cardiovascular);
        
    public static readonly HealthMilestone StrokeRiskReduced = new(
        "Stroke Risk Reduced",
        "Your stroke risk is reduced to that of a non-smoker.",
        Duration.FromDays(5 * 365),
        "5 years",
        "🧠",
        HealthCategory.Cardiovascular);
        
    public static readonly HealthMilestone LungCancerRiskHalved = new(
        "Lung Cancer Risk Halved",
        "Risk of lung cancer drops to half that of a smoker.",
        Duration.FromDays(10 * 365),
        "10 years",
        "🎗️",
        HealthCategory.CancerPrevention);
        
    public static readonly HealthMilestone HeartDiseaseRiskNormal = new(
        "Heart Disease Risk Normal",
        "Risk of coronary heart disease same as non-smoker.",
        Duration.FromDays(15 * 365),
        "15 years",
        "💖",
        HealthCategory.Cardiovascular);
    
    private static readonly HealthMilestone[] AllMilestones = {
        BloodPressureNormalizes, CarbonMonoxideDrops, HeartAttackRiskDecreases,
        NerveEndingsRegenerate, BreathingImproves, CirculationImproves,
        LungFunctionIncreases, CiliaRegenerate, EnergyLevelsIncrease,
        HeartDiseaseRiskHalved, StrokeRiskReduced, LungCancerRiskHalved, HeartDiseaseRiskNormal
    };
    
    public static IReadOnlyList<HealthMilestone> GetAll() => AllMilestones;
    
    public bool IsAchieved(Duration timeSinceQuit) => timeSinceQuit.IsGreaterThanOrEqual(TimeRequired);
    
    public double GetProgress(Duration timeSinceQuit) 
        => Math.Min(100, (double)timeSinceQuit.TotalMinutes / TimeRequired.TotalMinutes * 100);
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Title;
        yield return TimeRequired.TotalMinutes;
    }
}

public enum HealthCategory
{
    Cardiovascular,
    Respiratory,
    Sensory,
    Energy,
    CancerPrevention
}
