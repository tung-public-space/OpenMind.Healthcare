using DDD.BuildingBlocks;

namespace QuitSmokingApi.Domain.ValueObjects;

/// <summary>
/// Value object representing money - immutable and encapsulates money operations
/// </summary>
public class Money : ValueObject
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "USD";
    
    // Private parameterless constructor for EF Core
    private Money() { }
    
    private Money(decimal amount, string currency = "USD")
    {
        Amount = Math.Round(amount, 2);
        Currency = currency;
    }
    
    public static Money Create(decimal amount, string currency = "USD")
    {
        if (amount < 0)
            throw new DomainException("Money amount cannot be negative");
            
        return new Money(amount, currency);
    }
    
    public static Money Zero(string currency = "USD") => new(0, currency);
    
    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new DomainException("Cannot add money with different currencies");
            
        return new Money(Amount + other.Amount, Currency);
    }
    
    public Money Subtract(Money other)
    {
        if (Currency != other.Currency)
            throw new DomainException("Cannot subtract money with different currencies");
            
        return new Money(Amount - other.Amount, Currency);
    }
    
    public Money Multiply(decimal factor)
    {
        return new Money(Amount * factor, Currency);
    }
    
    public override string ToString() => $"{Currency} {Amount:F2}";
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
    
    // Implicit conversion for EF Core
    public static implicit operator decimal(Money money) => money.Amount;
}
