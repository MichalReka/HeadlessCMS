using HeadlessCMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HeadlessCMS.Persistence.Repositories
{
    public interface IBaseEntityRepository
    {
        public void Add<TEntity>(DbSet<TEntity> dbSet, TEntity entity, ClaimsPrincipal user) where TEntity : BaseEntity;
        public void Update<TEntity>(DbSet<TEntity> dbSet, TEntity entity, ClaimsPrincipal user) where TEntity : BaseEntity;

    }
}
