using DDD.BuildingBlocks;

namespace QuitSmokingApi.Domain.ValueObjects;

/// <summary>
/// Value object representing smoking habits - immutable
/// </summary>
public class SmokingHabits : ValueObject
{
    public int CigarettesPerDay { get; private set; }
    public int CigarettesPerPack { get; private set; }
    public Money PricePerPack { get; private set; } = null!;
    
    // Private parameterless constructor for EF Core
    private SmokingHabits() { }
    
    private SmokingHabits(int cigarettesPerDay, int cigarettesPerPack, Money pricePerPack)
    {
        CigarettesPerDay = cigarettesPerDay;
        CigarettesPerPack = cigarettesPerPack;
        PricePerPack = pricePerPack;
    }
    
    public static SmokingHabits Create(int cigarettesPerDay, int cigarettesPerPack, decimal pricePerPack)
    {
        if (cigarettesPerDay <= 0)
            throw new DomainException("Cigarettes per day must be greater than zero");
            
        if (cigarettesPerPack <= 0)
            throw new DomainException("Cigarettes per pack must be greater than zero");
            
        if (pricePerPack <= 0)
            throw new DomainException("Price per pack must be greater than zero");
            
        return new SmokingHabits(cigarettesPerDay, cigarettesPerPack, Money.Create(pricePerPack));
    }
    
    public Money PricePerCigarette => Money.Create(PricePerPack.Amount / CigarettesPerPack);
    
    public Money DailyCost => Money.Create(PricePerCigarette.Amount * CigarettesPerDay);
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CigarettesPerDay;
        yield return CigarettesPerPack;
        yield return PricePerPack;
    }
}
