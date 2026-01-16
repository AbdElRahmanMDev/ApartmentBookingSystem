using Domain.Shared;

namespace Domain.Apartments;

public record PricingDetails(Money priceForPeriod, Money CleaningFee, Money amenitiesUpCharge, Money TotalPerice);
