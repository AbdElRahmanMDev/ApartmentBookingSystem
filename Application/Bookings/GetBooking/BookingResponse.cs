namespace Application.Bookings.GetBooking
{
    public sealed class BookingResponse
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ApartmentId { get; set; }

        public int Status { get; set; }

        public Decimal PriceAmount { get; set; }

        public string PriceCurrency { get; set; }

        public decimal CleaningFeeAmount { get; set; }

        public string CleaningFeeCurrency { get; set; }

        public decimal AmenitiesUpChargeAmount { get; set; }

        public string AmenitiesUpChargeCurrency { get; set; }

        public decimal TotalPrice { get; set; }

        public string TotalPriceCurrency { get; set; }

        public DateTime CreateOnUtc { get; set; }

        public DateOnly DurationEnd { get; set; }

        public DateOnly DurationStart { get; set; }


    }
}
