using Domain.Apartments;
using Domain.Bookings;
using Domain.Bookings.Enums;
using Domain.Shared;
using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public sealed class BookingConfigurations : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("bookings");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.ApartmentId).IsRequired();
        builder.Property(b => b.UserId).IsRequired();

        builder.Property(b => b.Status)
            .HasConversion(
                s => s.ToString(),
                s => (BookingStatus)Enum.Parse(typeof(BookingStatus), s))
            .HasMaxLength(30)
            .IsRequired();

        // Postgres timestamptz (UTC)
        builder.Property(b => b.CreatedOnUtc)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(b => b.ConfirmedOnUtc).HasColumnType("timestamp with time zone");
        builder.Property(b => b.RejectedOnUtc).HasColumnType("timestamp with time zone");
        builder.Property(b => b.CanceledOnUtc).HasColumnType("timestamp with time zone");
        builder.Property(b => b.CompletedOnUtc).HasColumnType("timestamp with time zone");

        // -----------------------------
        // Owned: Duration (DateOnly)
        // -----------------------------
        builder.OwnsOne(b => b.Duration, db =>
        {
            db.Property(d => d.StartDate)
              .HasColumnName("Duration_StartDate")
              .IsRequired();

            db.Property(d => d.EndDate)
              .HasColumnName("Duration_EndDate")
              .IsRequired();

            db.WithOwner();
        });

        builder.Navigation(b => b.Duration).IsRequired();

        // -----------------------------
        // Owned: Money (class)
        // -----------------------------
        builder.OwnsOne(b => b.PriceForPeriod, mb =>
        {
            ConfigureMoney(mb, "PriceForPeriod");
            mb.WithOwner();
        });

        builder.OwnsOne(b => b.AmenitiesUpCharge, mb =>
        {
            ConfigureMoney(mb, "AmenitiesUpCharge");
            mb.WithOwner();
        });

        builder.OwnsOne(b => b.CleaningFee, mb =>
        {
            ConfigureMoney(mb, "CleaningFee");
            mb.WithOwner();
        });

        builder.OwnsOne(b => b.TotalPrice, mb =>
        {
            ConfigureMoney(mb, "TotalPrice");
            mb.WithOwner();
        });

        // ✅ Force owned navigations to always exist (prevents NULL inserts)
        builder.Navigation(b => b.PriceForPeriod).IsRequired();
        builder.Navigation(b => b.AmenitiesUpCharge).IsRequired();
        builder.Navigation(b => b.CleaningFee).IsRequired();
        builder.Navigation(b => b.TotalPrice).IsRequired();

        // -----------------------------
        // FKs
        // -----------------------------
        builder.HasOne<Apartment>()
            .WithMany()
            .HasForeignKey(b => b.ApartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureMoney(OwnedNavigationBuilder<Booking, Money> money, string prefix)
    {
        money.Property(m => m.Amount)
            .HasColumnName($"{prefix}_Amount")
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        money.Property(m => m.Currency)
            .HasColumnName($"{prefix}_Currency")
            .HasConversion(c => c.Code, code => Currency.FromCode(code))
            .HasMaxLength(10)
            .IsRequired();
    }
}
