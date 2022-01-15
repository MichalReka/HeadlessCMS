using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HeadlessCMS.Persistence.Repositories
{
    public class BaseEntityRepository : IBaseEntityRepository
    {
        private IUserService _userService;
        private ApplicationDbContext _applicationDbContext;

        public BaseEntityRepository(IUserService userService, ApplicationDbContext applicationDbContext)
        {
            _userService = userService;
            _applicationDbContext = applicationDbContext;
        }

        public void Add<TEntity>(TEntity entity, ClaimsPrincipal user) where TEntity : BaseEntity
        {
            var dbSet = _applicationDbContext.Set<TEntity>();

            var userId = new Guid(_userService.GetCurrentUserId(user));
            var userRecord = GetUserFromId(userId);

            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;
            entity.CreatedById = userId;
            entity.CreatedBy = userRecord;
            entity.UpdatedById = userId;
            entity.UpdatedBy = userRecord;
            dbSet.Add(entity);
        }

        public void Update<TEntity>(TEntity entity, ClaimsPrincipal user) where TEntity : BaseEntity
        {
            var dbSet = _applicationDbContext.Set<TEntity>();

            var userId = new Guid(_userService.GetCurrentUserId(user));
            var userRecord = GetUserFromId(userId);

            entity.UpdatedDate = DateTime.Now;
            entity.UpdatedById = userId;
            entity.UpdatedBy = userRecord;
            dbSet.Update(entity);
        }

        private User? GetUserFromId(Guid userId)
        {
            var userDbSet = _applicationDbContext.Set<User>();
            return userDbSet.Find(userId);
        }
    }
}
