using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HeadlessCMS.Persistence.Repositories
{
    public class BaseEntityRepository : IBaseEntityRepository
    {
        private IUserService _userService;
        public BaseEntityRepository(IUserService userService)
        {
            _userService = userService;
        }
        public void Add<TEntity>(DbSet<TEntity> dbSet, TEntity entity, ClaimsPrincipal user) where TEntity : BaseEntity
        {
            var userId = new Guid(_userService.GetCurrentUserId(user));
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;
            entity.CreatedBy = userId;
            entity.UpdatedBy = userId;
            dbSet.Add(entity);
        }
        public void Update<TEntity>(DbSet<TEntity> dbSet, TEntity entity, ClaimsPrincipal user) where TEntity : BaseEntity
        {
            var userId = new Guid(_userService.GetCurrentUserId(user));
            entity.UpdatedDate = DateTime.Now;
            entity.UpdatedBy = userId;
            dbSet.Update(entity);
        }
    }
}
