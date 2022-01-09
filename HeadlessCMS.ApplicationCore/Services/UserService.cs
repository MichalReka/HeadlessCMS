using System.Security.Claims;


namespace HeadlessCMS.ApplicationCore.Services
{
    public class UserService : IUserService
    {
        public string GetCurrentUserId(ClaimsPrincipal user)
        {
            return user.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
