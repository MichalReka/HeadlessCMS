using HeadlessCMS.ApplicationCore.Core.Interfaces.Providers;
using Microsoft.EntityFrameworkCore;

namespace HeadlessCMS.Persistence
{
    public class DbContextProvider : IDbContextProvider
    {
        private readonly ApplicationDbContext _context;

        public DbContextProvider(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}