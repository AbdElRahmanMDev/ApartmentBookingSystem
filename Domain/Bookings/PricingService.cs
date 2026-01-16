using Domain.Apartments;
using Domain.Shared;

namespace Domain.Bookings;

public class PricingService
{
    public PricingDetails CalculatePrice(Apartment apartments, DateRange period)
    {
        var currency = apartments.Price.Currency;

        var priceForPeriod = new Money(apartments.Price.Amount * period.LenghtInDays, currency);

        decimal percentageUpCharge = 0;

        foreach (var amenity in apartments.Amenities)
        {
            percentageUpCharge += amenity switch
            {
                Amenity.Garden or Amenity.Mountainview => 0.1m,
                Amenity.Parking => 0.05m,
                Amenity.AirConditioning => 0.01m,
                _ => 0
            };
        }

        var amenitiesUpCharge = Money.Zero(currency);
        if (percentageUpCharge > 0)
        {
            amenitiesUpCharge = new Money(priceForPeriod.Amount * percentageUpCharge, currency);
        }

        var totalPrice = Money.Zero();

        totalPrice += priceForPeriod;

        if (!apartments.CleaningFee.IsZero())
        {
            totalPrice += apartments.CleaningFee;

        }

        totalPrice += amenitiesUpCharge;

        return new PricingDetails(priceForPeriod, apartments.CleaningFee, amenitiesUpCharge, totalPrice);



    }

}
