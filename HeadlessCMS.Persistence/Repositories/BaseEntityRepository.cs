using AutoMapper;
using HeadlessCMS.ApplicationCore.Dtos;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HeadlessCMS.Persistence.Repositories
{
    public class BaseEntityRepository : IBaseEntityRepository
    {
        private ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;


        public BaseEntityRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public TEntity Add<TEntity>(TEntity entity, ClaimsPrincipal user) where TEntity : BaseEntity
        {
            var dbSet = _applicationDbContext.Set<TEntity>();

            var userId = new Guid(user.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            var userRecord = GetUserFromId(userId);

            entity.Id = Guid.NewGuid();
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;
            entity.CreatedById = userId;
            entity.UpdatedById = userId;
            entity.OwnerId = userId;
            return dbSet.Add(entity).Entity;
        }

        public TEntity Update<TEntity>(TEntity entity, ClaimsPrincipal user) where TEntity : BaseEntity
        {
            var dbSet = _applicationDbContext.Set<TEntity>();

            var userId = new Guid(user.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            var updatingUserRecord = GetUserFromId(userId);
            var originalCreatedUser = GetUserFromId(entity.CreatedById);

            entity.UpdatedDate = DateTime.Now;
            entity.UpdatedById = userId;
            entity.UpdatedBy = updatingUserRecord;
            entity.CreatedBy = originalCreatedUser;
            return dbSet.Update(entity).Entity;
        }

        private User? GetUserFromId(Guid userId)
        {
            var userDbSet = _applicationDbContext.Set<User>();
            return userDbSet.AsNoTracking().First(user => user.Id==userId);
        }
    }
}
