using HeadlessCMS.ApplicationCore.Dtos;
using HeadlessCMS.Domain.Entities;
using System.Security.Claims;

namespace HeadlessCMS.ApplicationCore.Core.Interfaces.Services
{
    public interface IUserAuthService
    {
        Task<Tokens> RefreshAsync(Claim userClaim, Claim refreshClaim);
        Task<Tokens> LoginAsync(Authentication authentication);
        User HandleRegister(UserDto userDto);
    }
}