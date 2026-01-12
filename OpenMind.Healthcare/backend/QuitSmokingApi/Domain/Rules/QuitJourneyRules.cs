using DDD.BuildingBlocks;

namespace QuitSmokingApi.Domain.Rules;

/// <summary>
/// Business rule: Quit date cannot be in the future
/// </summary>
public class QuitDateCannotBeInFutureRule : IBusinessRule
{
    private readonly DateTime _quitDate;
    
    public QuitDateCannotBeInFutureRule(DateTime quitDate)
    {
        _quitDate = quitDate;
    }
    
    public string RuleName => nameof(QuitDateCannotBeInFutureRule);
    
    public string ErrorMessage => "Quit date cannot be in the future";
    
    public bool IsBroken() => _quitDate > DateTime.UtcNow;
}

/// <summary>
/// Business rule: Cigarettes per day must be positive
/// </summary>
public class CigarettesPerDayMustBePositiveRule : IBusinessRule
{
    private readonly int _cigarettesPerDay;
    
    public CigarettesPerDayMustBePositiveRule(int cigarettesPerDay)
    {
        _cigarettesPerDay = cigarettesPerDay;
    }
    
    public string RuleName => nameof(CigarettesPerDayMustBePositiveRule);
    
    public string ErrorMessage => "Cigarettes per day must be greater than zero";
    
    public bool IsBroken() => _cigarettesPerDay <= 0;
}

/// <summary>
/// Business rule: Price per pack must be positive
/// </summary>
public class PricePerPackMustBePositiveRule : IBusinessRule
{
    private readonly decimal _pricePerPack;
    
    public PricePerPackMustBePositiveRule(decimal pricePerPack)
    {
        _pricePerPack = pricePerPack;
    }
    
    public string RuleName => nameof(PricePerPackMustBePositiveRule);
    
    public string ErrorMessage => "Price per pack must be greater than zero";
    
    public bool IsBroken() => _pricePerPack <= 0;
}
