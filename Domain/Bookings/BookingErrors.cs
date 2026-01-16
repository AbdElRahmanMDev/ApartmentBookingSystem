using Domain.Abstraction;
namespace Domain.Bookings
{
    public static class BookingErrors
    {
        public static Error NotFound = new("Booking.NotFound", "The booking was not found.");

        public static Error NotPending = new("Booking.NotPending", "The booking is not in a pending state to perform this operation.");

        public static Error Overlap = new("Booking.Overlap", "The booking overlaps with an existing booking for the same apartment.");

        public static Error NotReserved = new("Booking.NotReserved", "The booking is not in a reserved state to perform this operation.");

        public static Error NotConfirmed = new("Booking.NotConfirmed", "The booking is not in a confirmed state to perform this operation.");

        public static Error AlreadyStarted = new("Booking.AlreadyStarted", "The booking has already started.");


    }
}
