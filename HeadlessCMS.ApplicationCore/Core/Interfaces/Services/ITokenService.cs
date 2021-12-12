using HeadlessCMS.Domain.Entities;

namespace HeadlessCMS.ApplicationCore.Core.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        (string refreshToken, string jwt) GenerateRefreshToken(User user);
    }
}