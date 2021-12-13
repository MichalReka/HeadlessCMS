using Microsoft.EntityFrameworkCore;

namespace HeadlessCMS.Domain.Interfaces
{
    public interface IApplicationDbContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        Task<int> SaveChangesAsync();
    }
}
