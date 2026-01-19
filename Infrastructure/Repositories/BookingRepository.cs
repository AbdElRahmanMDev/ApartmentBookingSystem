using Domain.Apartments;
using Domain.Bookings;
using Domain.Bookings.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookingRepository : Repository<Booking>, IBookingRepository

    {
        private static readonly BookingStatus[] ActiveBookingStatuses =
        {
            BookingStatus.Confirmed,
            BookingStatus.Completed,
            BookingStatus.Reserved
        };
        public BookingRepository(ApplicationDbContext context) : base(context)
        {
        }


        public async Task<bool> IsOverlappingAsync(Apartment apartment, DateRange dateRange, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Booking>().AnyAsync(booking => booking.ApartmentId == apartment.Id
            && booking.Duration.StartDate < dateRange.EndDate
            && booking.Duration.EndDate > dateRange.StartDate
            && ActiveBookingStatuses.Contains(booking.Status), cancellationToken);

        }


    }
}
