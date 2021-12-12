using HeadlessCMS.Domain.Entities;
using System.Security.Claims;

namespace HeadlessCMS.ApplicationCore.Core.Interfaces.Services
{
    public interface IUserAuthService
    {
        Tokens Refresh(Claim userClaim, Claim refreshClaim);
        Tokens Login(Authentication authentication);

    }
}