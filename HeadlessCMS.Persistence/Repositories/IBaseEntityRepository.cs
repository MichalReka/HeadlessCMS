using HeadlessCMS.Domain.Entities;
using System.Security.Claims;

namespace HeadlessCMS.Persistence.Repositories
{
    public interface IBaseEntityRepository
    {
        public TEntity Add<TEntity>(TEntity entity, ClaimsPrincipal user) where TEntity : BaseEntity;

        public TEntity Update<TEntity>(TEntity entity, ClaimsPrincipal user) where TEntity : BaseEntity;
    }
}