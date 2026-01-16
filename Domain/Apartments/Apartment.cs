using Domain.Abstraction;
using Domain.Shared;

namespace Domain.Apartments;

public sealed class Apartment : Entity
{
    public Apartment(Guid Id,
        Name name,
        Description description,
        Address address,
        Money price,
        Money cleaningFee,
        List<Amenity> amenities) : base(Id)
    {
        Name = name;
        Description = description;
        Address = address;
        Price = price;
        CleaningFee = cleaningFee;
        Amenities = amenities;

    }

    public Address Address { get; private set; }

    public Name Name { get; private set; }

    public Money Price { get; private set; }

    public Money CleaningFee { get; private set; }


    public DateTime? LastBookOnUtc { get; internal set; }
    public Description Description { get; private set; }

    public List<Amenity> Amenities { get; private set; } = new List<Amenity>();
}
