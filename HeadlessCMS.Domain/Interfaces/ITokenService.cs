using HeadlessCMS.Domain.Entities;

namespace HeadlessCMS.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        (string refreshToken, string jwt) GenerateRefreshToken(User user);
    }
}