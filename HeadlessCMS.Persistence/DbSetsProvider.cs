using HeadlessCMS.ApplicationCore.Core.Interfaces.Providers;
using Microsoft.EntityFrameworkCore;

namespace HeadlessCMS.Persistence
{
    public class DbSetsProvider : IDbSetsProvider
    {
        private readonly ApplicationDbContext _context;

        public DbSetsProvider(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }
    }
}