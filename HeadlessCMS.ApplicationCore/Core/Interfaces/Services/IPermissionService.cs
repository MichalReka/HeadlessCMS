using HeadlessCMS.Domain.Entities;
using System.Security.Claims;

namespace HeadlessCMS.ApplicationCore.Core.Interfaces.Services
{
    public interface IPermissionService
    {
        void ValidateOwnership<TEntity>(ClaimsPrincipal user, string id) where TEntity : BaseEntity;
        void ValidateIfAdmin(ClaimsPrincipal user);
        void ValidateIfCanChangeUser(ClaimsPrincipal user, Guid changedUserId);
    }
}