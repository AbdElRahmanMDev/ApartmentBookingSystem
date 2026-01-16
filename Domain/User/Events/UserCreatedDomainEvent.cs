using Domain.Abstraction;

namespace Domain.User.Events;

internal record UserCreatedDomainEvent(Guid UserId) : IDomainEvents;

