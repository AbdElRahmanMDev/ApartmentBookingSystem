namespace Domain.Shared;

public sealed record Currency(string Code)
{
    public static readonly Currency USD = new("USD");
    public static readonly Currency EUR = new("EUR");

    // If you need "None", keep it internal but DO NOT persist it to DB.
    internal static readonly Currency None = new("");

    public static Currency FromCode(string code) =>
        code switch
        {
            "USD" => USD,
            "EUR" => EUR,
            "" => None,
            _ => throw new ApplicationException($"Currency not found: '{code}'")
        };
}
