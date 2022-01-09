using System.Security.Claims;

namespace HeadlessCMS.ApplicationCore.Services
{
    public interface IUserService
    {
        public string GetCurrentUserId(ClaimsPrincipal user);
    }
}