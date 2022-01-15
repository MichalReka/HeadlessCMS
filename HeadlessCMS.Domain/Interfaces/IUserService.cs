using System.Security.Claims;

namespace HeadlessCMS.Domain.Interfaces
{
    public interface IUserService
    {
        public string GetCurrentUserId(ClaimsPrincipal user);
    }
}