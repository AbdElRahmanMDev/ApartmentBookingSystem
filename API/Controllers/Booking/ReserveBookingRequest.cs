namespace API.Controllers.Booking
{
    public record ReserveBookingRequest(Guid ApartmentId,
    Guid UserId,
    DateOnly StartDate,
    DateOnly EndDate);

}
