using HeadlessCMS.Domain.Entities;
using System.Security.Claims;

namespace HeadlessCMS.Persistence.Repositories
{
    public interface IBaseEntityRepository
    {
        public void Add<TEntity>(TEntity entity, ClaimsPrincipal user) where TEntity : BaseEntity;

        public void Update<TEntity>(TEntity entity, ClaimsPrincipal user) where TEntity : BaseEntity;
    }
}