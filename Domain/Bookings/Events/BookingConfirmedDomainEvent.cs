using Domain.Abstraction;

namespace Domain.Bookings.Events
{
    public record BookingConfirmedDomainEvent(Guid Id) : IDomainEvents;

}
