using Domain.Abstraction;

namespace Domain.Bookings.Events
{
    public record BookingCompletedDomainEvent(Guid BookingId) : IDomainEvents;

}
