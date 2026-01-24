// Domain/Bookings/PricingService.cs
// IMPORTANT: Guarantees CleaningFee is NEVER null AND NEVER reused as the same instance (Owned Types rule)

using Domain.Apartments;
using Domain.Shared;

namespace Domain.Bookings;

public class PricingService
{
    public PricingDetails CalculatePrice(Apartment apartment, DateRange period)
    {
        var currency = apartment.Price.Currency;

        var priceForPeriod = new Money(apartment.Price.Amount * period.LenghtInDays, currency);

        decimal percentageUpCharge = 0m;
        foreach (var amenity in apartment.Amenities)
        {
            percentageUpCharge += amenity switch
            {
                Amenity.Garden or Amenity.Mountainview => 0.1m,
                Amenity.Parking => 0.05m,
                Amenity.AirConditioning => 0.01m,
                _ => 0m
            };
        }

        var amenitiesUpCharge = Money.Zero(currency);
        if (percentageUpCharge > 0m)
        {
            amenitiesUpCharge = new Money(priceForPeriod.Amount * percentageUpCharge, currency);
        }

        // ✅ CHANGE (IMPORTANT):
        // Do NOT reuse apartment.CleaningFee instance directly.
        // EF Core Owned Types cannot be shared between multiple owners.
        // When many bookings reference the same Apartment, they would all share the same Money instance
        // -> EF tracking can null-out the owned navigation causing NULL insert into CleaningFee_Amount.
        //
        // So we clone it into a NEW Money instance every time.
        var cleaningFee = apartment.CleaningFee is null
            ? Money.Zero(currency)
            : new Money(apartment.CleaningFee.Amount, apartment.CleaningFee.Currency);

        var totalPrice = Money.Zero(currency);
        totalPrice += priceForPeriod;

        // ✅ Use the cloned cleaningFee (not apartment.CleaningFee)
        if (!cleaningFee.IsZero())
        {
            totalPrice += cleaningFee;
        }

        totalPrice += amenitiesUpCharge;

        // ✅ Return cloned cleaningFee (not apartment.CleaningFee)
        return new PricingDetails(
            priceForPeriod,
            cleaningFee,
            amenitiesUpCharge,
            totalPrice);
    }
}
