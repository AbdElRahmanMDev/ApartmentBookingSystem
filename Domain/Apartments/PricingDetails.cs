using Domain.Shared;

namespace Domain.Apartments;

public sealed record PricingDetails(
    Money PriceForPeriod,
    Money CleaningFee,
    Money AmenitiesUpCharge,
    Money TotalPrice
);