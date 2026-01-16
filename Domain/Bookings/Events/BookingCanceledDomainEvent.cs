using Domain.Abstraction;

namespace Domain.Bookings.Events
{
    public record BookingCanceledDomainEvent(Guid BookingId) : IDomainEvents;

}
