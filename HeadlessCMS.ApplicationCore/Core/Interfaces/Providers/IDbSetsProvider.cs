using Microsoft.EntityFrameworkCore;

namespace HeadlessCMS.ApplicationCore.Core.Interfaces.Providers
{
    public interface IDbContextProvider
    {
        DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class;
        Task SaveAsync();
    }
}