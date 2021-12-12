using Microsoft.EntityFrameworkCore;

namespace HeadlessCMS.ApplicationCore.Core.Interfaces.Providers
{
    public interface IDbSetsProvider
    {
        DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class;
    }
}