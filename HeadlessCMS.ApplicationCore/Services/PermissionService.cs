using HeadlessCMS.ApplicationCore.Core.Interfaces.Services;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security;
using System.Security.Claims;

namespace HeadlessCMS.ApplicationCore.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PermissionService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void ValidateOwnership<TEntity>(ClaimsPrincipal user, string entityId) where TEntity : BaseEntity
        {
            var userId = user.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                var foundUser = _applicationDbContext.Set<User>().AsNoTracking().FirstOrDefault(user => user.Id.ToString() == userId);

                if (foundUser != null)
                {
                    if (foundUser.Role == UserRole.Admin)
                    {
                        return;
                    }

                    var foundEntity = _applicationDbContext.Set<TEntity>().AsNoTracking().FirstOrDefault(entity => entity.Id.ToString() == entityId);

                    if (foundUser != null && foundEntity != null && foundEntity.OwnerId == foundUser.Id)
                    {
                        return;
                    }
                }
            }
            else
            {
                throw new SecurityException("Niepoprawnie utworzony token");
            }
            throw new SecurityException("Użytkownik nie ma wystarczających uprawnień do wykonania tej akcji");
        }

        public void ValidateIfCanChangeUser(ClaimsPrincipal user, Guid changedUserId)
        {
            var userId = user.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                var foundUser = _applicationDbContext.Set<User>().AsNoTracking().FirstOrDefault(user => user.Id.ToString() == userId);

                if (foundUser != null)
                {
                    if (foundUser.Role == UserRole.Admin || foundUser.Id == changedUserId)
                    {
                        return;
                    }
                }
            }
            else
            {
                throw new SecurityException("Niepoprawnie utworzony token");
            }
            throw new SecurityException("Użytkownik nie ma wystarczających uprawnień do wykonania tej akcji");
        }

        public void ValidateIfAdmin(ClaimsPrincipal user)
        {
            var userId = user.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var foundUser = _applicationDbContext.Set<User>().FirstOrDefault(user => user.Id.ToString() == userId);

            if (foundUser != null && foundUser.Role == UserRole.Admin)
            {
                return;
            }

            throw new SecurityException("Użytkownik nie ma wystarczających uprawnień do wykonania tej akcji");
        }
    }
}