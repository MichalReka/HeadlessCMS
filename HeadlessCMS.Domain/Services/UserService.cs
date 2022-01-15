using HeadlessCMS.Domain.Interfaces;
using System.Security.Claims;


namespace HeadlessCMS.Domain.Services
{
    public class UserService : IUserService
    {
        public string GetCurrentUserId(ClaimsPrincipal user)
        {
            return user.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
