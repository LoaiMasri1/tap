using Tap.Domain.Core.Primitives;

namespace Tap.Domain.Common.ValueObjects;

public class Money : ValueObject
{
    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, string currency)
    {
        return new Money(amount, currency);
    }

    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    public static Money operator +(Money money1, Money money2)
    {
        if (money1.Currency != money2.Currency)
        {
            throw new InvalidOperationException("Cannot sum amounts with different currencies");
        }

        return Create(money1.Amount + money2.Amount, money1.Currency);
    }

    public static Money operator -(Money money1, Money money2)
    {
        if (money1.Currency != money2.Currency)
        {
            throw new InvalidOperationException(
                "Cannot subtract amounts with different currencies"
            );
        }

        return Create(money1.Amount - money2.Amount, money1.Currency);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Amount;
        yield return Currency;
    }
}
