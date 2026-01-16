namespace Domain.Shared;

public record Currency
{

    public static readonly Currency USD = new("USD");

    public static readonly Currency EUR = new("EUR");

    internal static readonly Currency None = new("");
    public string Code { get; init; }

    private Currency(string code) => Code = code;

    public static Currency FromCode(string Code)
    {
        return GetAll.First(x => x.Code == Code) ?? throw new ApplicationException("Currency not found");
    }
    public static IReadOnlyCollection<Currency> GetAll => new[] { USD, EUR };

}
