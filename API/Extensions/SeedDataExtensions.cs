using Bogus;
using Domain.Apartments;
using Domain.Bookings;
using Domain.Shared;
using Domain.User;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class SeedDataExtensions
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var pricingService = scope.ServiceProvider.GetRequiredService<PricingService>();


            SeedAsync(context, pricingService).GetAwaiter().GetResult();
        }

        public static async Task SeedAsync(
            ApplicationDbContext context,
            PricingService pricingService,
            int usersCount = 50,
            int apartmentsCount = 30,
            int bookingsCount = 120)
        {
            // ✅ Skip ONLY if ALL are already seeded (prevents half-seeded state)
            if (await context.Set<User>().AnyAsync() &&
                await context.Set<Apartment>().AnyAsync() &&
                await context.Set<Booking>().AnyAsync())
            {
                return;
            }

            // -----------------------------
            // 1) USERS (seed only if missing)
            // -----------------------------
            List<User> users;
            if (!await context.Set<User>().AnyAsync())
            {
                var userFaker = new Faker<User>()
                    .CustomInstantiator(f =>
                    {
                        var first = new FirstName(f.Name.FirstName());
                        var last = new LastName(f.Name.LastName());

                        // ensure unique email by embedding a guid part
                        var emailValue = f.Internet.Email(first.Value, last.Value).ToLowerInvariant();
                        emailValue = $"{Guid.NewGuid():N}_{emailValue}";

                        var email = new Email(emailValue);
                        return User.Create(first, last, email);
                    });

                users = userFaker.Generate(usersCount);

                context.AddRange(users);
                await context.SaveChangesAsync();
            }
            else
            {
                users = await context.Set<User>().AsNoTracking().ToListAsync();
            }

            // -----------------------------
            // 2) APARTMENTS (seed only if missing)
            // -----------------------------
            List<Apartment> apartments;

            Currency PickCurrency(Faker _) => Currency.USD;

            List<Amenity> RandomAmenities(Faker f)
            {
                var all = Enum.GetValues<Amenity>().ToList();
                var count = f.Random.Int(2, 7);

                return f.Random
                    .Shuffle(all)
                    .Take(count)
                    .ToList();
            }

            if (!await context.Set<Apartment>().AnyAsync())
            {
                var apartmentFaker = new Faker<Apartment>()
                    .CustomInstantiator(f =>
                    {
                        var id = Guid.NewGuid();

                        var name = new Name($"{f.Commerce.ProductAdjective()} apartment in {f.Address.City()}");
                        var description = new Description(f.Lorem.Paragraphs(2));

                        var address = new Address(
                            Country: f.Address.Country(),
                            State: f.Address.State(),
                            ZipCode: f.Address.ZipCode(),
                            City: f.Address.City(),
                            Street: f.Address.StreetAddress());

                        var currency = PickCurrency(f);

                        var price = new Money(f.Random.Decimal(30, 350), currency);

                        // ✅ IMPORTANT:
                        // Keep cleaning fee ALWAYS > 0 so Booking pricing never leaves CleaningFee NULL.
                        // (Your DB has NOT NULL on Booking.CleaningFee_Amount)
                        var cleaningFee = new Money(f.Random.Decimal(10, 60), currency);

                        var amenities = RandomAmenities(f);

                        return new Apartment(
                            Id: id,
                            name: name,
                            description: description,
                            address: address,
                            price: price,
                            cleaningFee: cleaningFee,
                            amenities: amenities
                        );
                    });

                apartments = apartmentFaker.Generate(apartmentsCount);

                context.AddRange(apartments);
                await context.SaveChangesAsync();
            }
            else
            {
                apartments = await context.Set<Apartment>().AsNoTracking().ToListAsync();
            }

            // -----------------------------
            // 3) BOOKINGS (seed only if missing)
            // -----------------------------
            if (await context.Set<Booking>().AnyAsync())
                return;

            static DateTime UtcStartOfDay(DateOnly date)
                => new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);

            static DateRange RandomDateRange(Faker f, DateTime createdUtc)
            {
                var startUtc = createdUtc.AddDays(f.Random.Int(1, 60));
                var start = DateOnly.FromDateTime(startUtc);

                var nights = f.Random.Int(1, 14);
                var end = start.AddDays(nights);

                return DateRange.Create(start, end);
            }

            var bookingFaker = new Faker<Booking>()
                .CustomInstantiator(f =>
                {
                    var apartment = f.PickRandom(apartments);
                    var user = f.PickRandom(users);

                    // ✅ UTC (Kind=Utc)
                    var createdUtc = f.Date.RecentOffset(180).UtcDateTime;

                    var duration = RandomDateRange(f, createdUtc);

                    var booking = Booking.Create(
                        apartment: apartment,
                        userId: user.Id,
                        duration: duration,
                        utcNow: createdUtc,
                        pricingService: pricingService
                    );

                    var roll = f.Random.Int(1, 100);

                    if (roll <= 55)
                    {
                        booking.Confirm(createdUtc.AddHours(f.Random.Int(1, 24)));
                    }
                    else if (roll <= 70)
                    {
                        booking.Reject(createdUtc.AddHours(f.Random.Int(1, 24)));
                    }
                    else if (roll <= 85)
                    {
                        booking.Confirm(createdUtc.AddHours(2));
                        booking.Cancel(createdUtc.AddHours(f.Random.Int(3, 12)));
                    }
                    else
                    {
                        booking.Confirm(createdUtc.AddHours(2));

                        // ✅ Complete AFTER end date, and UTC Kind
                        var completedUtc = UtcStartOfDay(duration.EndDate).AddDays(f.Random.Int(0, 10));
                        booking.Complete(completedUtc);
                    }

                    return booking;
                });

            List<Booking> bookings = bookingFaker.Generate(bookingsCount);
            var bad = bookings.Where(b => b.CleaningFee is null).ToList();
            if (bad.Any())
                throw new Exception($"Found {bad.Count} bookings with NULL CleaningFee before saving.");

            context.AddRange(bookings);
            var trackedBad = context.ChangeTracker
                    .Entries<Booking>()
                    .Where(e => e.State == EntityState.Added)
                    .Select(e => e.Entity)
                    .Where(b => b.CleaningFee is null)
                    .ToList();

            var trackedBad2 = context.ChangeTracker
                .Entries<Booking>()
                .Where(e => e.State == EntityState.Added)
                .Where(e => e.Reference(b => b.CleaningFee).CurrentValue == null)
                .ToList();


            await context.SaveChangesAsync();
        }
    }
}
