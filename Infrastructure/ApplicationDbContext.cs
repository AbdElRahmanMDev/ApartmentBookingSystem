using Application.Exceptions;
using Domain.Abstraction;
using Infrastructure.Configurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext, IUnitOfWork
    {
        private readonly IPublisher _publisher;


        public ApplicationDbContext(DbContextOptions option, IPublisher publisher) :
            base(option)
        {
            _publisher = publisher;

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingConfigurations).Assembly);
            base.OnModelCreating(modelBuilder);

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await base.SaveChangesAsync(cancellationToken);

                await PublishDomainEvents();

                return result;

            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException("Concurrency Issue", ex);
            }




        }


        private async Task PublishDomainEvents()
        {
            var domainEntities = ChangeTracker
                .Entries<Entity>()
                .Select(x => x.Entity)
                .SelectMany(x =>
                {
                    var events = x.GetDomainEvents();
                    x.ClearDomainEvents();
                    return events;
                })
                .ToList();

            foreach (var domainEvent in domainEntities)
            {
                await _publisher.Publish(domainEvent, cancellationToken: CancellationToken.None);
            }





        }
    }
}

