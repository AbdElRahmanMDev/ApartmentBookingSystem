using Domain.Abstraction;
using Domain.Apartments;
using Domain.Bookings.Enums;
using Domain.Bookings.Events;
using Domain.Shared;

namespace Domain.Bookings;

public sealed class Booking : Entity
{
    private Booking(
        Guid id,
        Guid apartmentId,
        Guid userId,
        DateRange duration,
        Money priceForPeriod,
        Money amenitiesUpCharge,
        Money cleaningFee,
        Money totalPrice,
        BookingStatus status,
        DateTime createdOnUtc)
        : base(id)
    {
        ApartmentId = apartmentId;
        UserId = userId;
        Duration = duration;

        PriceForPeriod = priceForPeriod ?? throw new ArgumentNullException(nameof(priceForPeriod));
        AmenitiesUpCharge = amenitiesUpCharge ?? throw new ArgumentNullException(nameof(amenitiesUpCharge));
        CleaningFee = cleaningFee ?? throw new ArgumentNullException(nameof(cleaningFee));
        TotalPrice = totalPrice ?? throw new ArgumentNullException(nameof(totalPrice));

        Status = status;
        CreatedOnUtc = createdOnUtc;
    }

    private Booking() { }

    public Guid ApartmentId { get; set; }
    public Guid UserId { get; set; }

    public DateRange Duration { get; private set; }

    public Money PriceForPeriod { get; private set; }
    public Money AmenitiesUpCharge { get; private set; }
    public Money CleaningFee { get; private set; }
    public Money TotalPrice { get; private set; }

    public BookingStatus Status { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ConfirmedOnUtc { get; private set; }
    public DateTime? RejectedOnUtc { get; private set; }
    public DateTime? CanceledOnUtc { get; private set; }
    public DateTime? CompletedOnUtc { get; private set; }

    public static Booking Create(
        Apartment apartment,
        Guid userId,
        DateRange duration,
        DateTime utcNow,
        PricingService pricingService)
    {
        var pricingDetails = pricingService.CalculatePrice(apartment, duration);

        var currency = apartment.Price.Currency;

        // ✅ IMPORTANT FIX:
        // EF Core Owned Types cannot be shared between multiple owners.
        // pricingService may return references like apartment.CleaningFee (same instance).
        // So we FORCE a NEW Money instance for each Booking to prevent EF from setting it to NULL during tracking.

        Money CloneOrZero(Money? m, Currency c) =>
            m is null ? Money.Zero(c) : new Money(m.Amount, m.Currency);

        var priceForPeriod = CloneOrZero(pricingDetails.PriceForPeriod, currency);
        var amenitiesUpCharge = CloneOrZero(pricingDetails.AmenitiesUpCharge, currency);
        var cleaningFee = CloneOrZero(pricingDetails.CleaningFee, currency);

        // total = new instances too (or compute fresh)
        var totalPrice = pricingDetails.TotalPrice is null
            ? (Money.Zero(currency) + priceForPeriod + amenitiesUpCharge + cleaningFee)
            : new Money(pricingDetails.TotalPrice.Amount, pricingDetails.TotalPrice.Currency);

        var booking = new Booking(
            id: Guid.NewGuid(),
            apartmentId: apartment.Id,
            userId: userId,
            duration: duration,
            priceForPeriod: priceForPeriod,
            amenitiesUpCharge: amenitiesUpCharge,
            cleaningFee: cleaningFee,
            totalPrice: totalPrice,
            status: BookingStatus.Reserved,
            createdOnUtc: utcNow
        );

        booking.RaiseDomainEvent(new BookingReservedDomainEvent(booking.Id));
        apartment.LastBookOnUtc = utcNow;

        return booking;
    }

    public Result Confirm(DateTime utcNow)
    {
        if (Status != BookingStatus.Reserved)
            return Result.Failure(BookingErrors.NotReserved);

        Status = BookingStatus.Confirmed;
        ConfirmedOnUtc = utcNow;

        RaiseDomainEvent(new BookingConfirmedDomainEvent(Id));
        return Result.Success();
    }

    public Result Reject(DateTime utcNow)
    {
        if (Status != BookingStatus.Reserved)
            return Result.Failure(BookingErrors.NotPending);

        Status = BookingStatus.Rejected;
        RejectedOnUtc = utcNow;

        RaiseDomainEvent(new BookingRejectedDomainEvent(Id));
        return Result.Success();
    }

    public Result Complete(DateTime utcNow)
    {
        if (Status != BookingStatus.Confirmed && Status != BookingStatus.Reserved)
            return Result.Failure(BookingErrors.NotPending);

        Status = BookingStatus.Completed;
        CompletedOnUtc = utcNow;

        RaiseDomainEvent(new BookingCompletedDomainEvent(Id));
        return Result.Success();
    }

    public Result Cancel(DateTime utcNow)
    {
        if (Status != BookingStatus.Confirmed)
            return Result.Failure(BookingErrors.NotPending);

        var currentDate = DateOnly.FromDateTime(utcNow);
        if (currentDate > Duration.StartDate)
            return Result.Failure(BookingErrors.AlreadyStarted);

        Status = BookingStatus.Cancelled;
        CanceledOnUtc = utcNow;

        RaiseDomainEvent(new BookingCanceledDomainEvent(Id));
        return Result.Success();
    }
}
