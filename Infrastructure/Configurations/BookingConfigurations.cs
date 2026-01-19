using Domain.Apartments;
using Domain.Bookings;
using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class BookingConfigurations : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("bookings");

        builder.HasKey(x => x.Id);

        builder.OwnsOne(booking => booking.PriceForPeriod, priceBuilder =>
        {
            priceBuilder.Property(money => money.Currency)
            .HasConversion(currency => currency.Code, code => Domain.Shared.Currency.FromCode(code));
        });

        builder.OwnsOne(booking => booking.CleaningFee, priceBuilder =>
        {
            priceBuilder.Property(money => money.Currency)
            .HasConversion(currency => currency.Code, code => Domain.Shared.Currency.FromCode(code));
        });
        builder.OwnsOne(booking => booking.AmenitiesUpCharge, priceBuilder =>
        {
            priceBuilder.Property(money => money.Currency)
            .HasConversion(currency => currency.Code, code => Domain.Shared.Currency.FromCode(code));
        });
        builder.OwnsOne(booking => booking.TotalPrice, priceBuilder =>
        {
            priceBuilder.Property(money => money.Currency)
.HasConversion(currency => currency.Code, code => Domain.Shared.Currency.FromCode(code));

        });

        builder.OwnsOne(booking => booking.Duration);

        builder.HasOne<Apartment>().WithMany().HasForeignKey(booking => booking.ApartmentId); //foreign key>

        builder.HasOne<User>().WithMany().HasForeignKey(booking => booking.UserId);
    }
}
