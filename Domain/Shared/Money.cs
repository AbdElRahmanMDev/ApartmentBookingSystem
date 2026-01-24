namespace Domain.Shared;

public sealed record Money(decimal Amount, Currency Currency)
{
    public static Money operator +(Money first, Money second)
    {
        if (first.Currency != second.Currency)
            throw new InvalidOperationException(
                $"Cannot add money of different currencies: '{first.Currency.Code}' vs '{second.Currency.Code}'");

        return new Money(first.Amount + second.Amount, first.Currency);
    }

    public static Money Zero(Currency currency) => new(0m, currency);
    public static Money Zero() => new(0m, Currency.None);
    // Optional helper: avoid calling Zero() without currency
    public bool IsZero() => Amount == 0m;
}
