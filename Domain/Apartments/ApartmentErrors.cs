using Domain.Abstraction;

namespace Domain.Apartments;

public class ApartmentErrors
{
    public static Error NotFound = new Error("Apartment.NotFound", "The apartment was not found.");
}
