using Domain.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class Repository<T> where T : Entity
    {
        internal readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }




        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)

        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }



        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }
    }
}
