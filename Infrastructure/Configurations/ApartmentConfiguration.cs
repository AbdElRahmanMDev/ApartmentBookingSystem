using Domain.Apartments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
    {
        public void Configure(EntityTypeBuilder<Apartment> builder)
        {
            builder.ToTable("apartments");

            builder.HasKey(x => x.Id);

            builder.OwnsOne(x => x.Address);

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .HasConversion(name => name.Value, value => new Name(value));

            builder.Property(x => x.Description)
                .HasMaxLength(1000)
                .HasConversion(description => description.Value, value => new Description(value));

            builder.OwnsOne(x => x.Price, priceBuilder =>
            {
                priceBuilder.Property(p => p.Currency).HasConversion(currency => currency.Code, value => Domain.Shared.Currency.FromCode(value));
            });

            builder.OwnsOne(a => a.CleaningFee, priceBuilder =>
            {
                priceBuilder.Property(p => p.Currency).HasConversion(currency => currency.Code, value => Domain.Shared.Currency.FromCode(value));
            });

            builder.Property<uint>("Version").IsRowVersion();
        }
    }
}
