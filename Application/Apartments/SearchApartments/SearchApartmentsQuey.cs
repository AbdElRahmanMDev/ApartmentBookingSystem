using Application.Abstraction.Messaging;

namespace Application.Apartments.SearchApartments;

public record SearchApartmentsQuey(DateOnly StartDate, DateOnly EndDate)

    : IQuery<IReadOnlyList<ApartmentResponse>>

    ;