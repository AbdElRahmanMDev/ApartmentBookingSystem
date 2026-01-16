using Domain.Abstraction;

namespace Domain.Bookings.Events;

public record BookingRejectedDomainEvent(Guid BookingId) : IDomainEvents;

