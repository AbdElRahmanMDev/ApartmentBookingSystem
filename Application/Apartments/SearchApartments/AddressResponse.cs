namespace Application.Apartments.SearchApartments
{
    public sealed class AddressResponse
    {
        public string Country { get; set; }

        public string State { get; init; }

        public string ZipCode { get; init; }

        public string City { get; init; }

        public string Street { get; init; }

    }
}
