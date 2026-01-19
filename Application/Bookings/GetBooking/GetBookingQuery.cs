using Application.Abstraction.Messaging;

namespace Application.Bookings.GetBooking
{
    public record GetBookingQuery(Guid BookingId) : IQuery<BookingResponse>
    {
    }
}
