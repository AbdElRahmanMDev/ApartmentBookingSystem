namespace Domain.Bookings
{
    public record DateRange
    {
        private DateRange()
        {

        }

        public DateOnly StartDate { get; init; }

        public DateOnly EndDate { get; init; }

        public int LenghtInDays => StartDate.DayNumber - EndDate.DayNumber;

        public static DateRange Create(DateOnly startDate, DateOnly endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be greater than end date");
            }


            return new DateRange { StartDate = startDate, EndDate = endDate };



        }



    }

}
