using HeadlessCMS.ApplicationCore.Core.Interfaces.Providers;
using HeadlessCMS.ApplicationCore.Core.Interfaces.Services;
using HeadlessCMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HeadlessCMS.ApplicationCore.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly DbSet<User> _users;
        private readonly ITokenService _tokenService;

        public UserAuthService(IDbSetsProvider dbSetsProvider, ITokenService tokenService)
        {
            _users=dbSetsProvider.GetDbSet<User>();
            _tokenService=tokenService;
        }

        public Tokens Login(Authentication authentication)
        {
            User user = _users.FirstOrDefault(u => u.Name == authentication.Name);

            if (user == null)
                throw new System.Exception("User doesn't exist");

            bool validPassword = user.Password == authentication.Password;

            if (validPassword)
            {
                var refreshToken = _tokenService.GenerateRefreshToken(user);

                //if (user.RefreshTokens == null)
                //    user.RefreshTokens = new List<string>();

                user.RefreshTokens = refreshToken.refreshToken;

                _users.Update(user);

                return new Tokens
                {
                    AccessToken = _tokenService.GenerateAccessToken(user),
                    RefreshToken = refreshToken.jwt
                };
            }
            else
            {
                throw new System.Exception("Username or password incorrect");
            }
        }

        public Tokens Refresh(Claim userClaim, Claim refreshClaim)
        {
            User user = _users.FirstOrDefault(x => x.Name == userClaim.Value);

            if (user == null)
                throw new System.Exception("User doesn't exist");

            //if (user.RefreshTokens == null)
            //    user.RefreshTokens = new List<string>();

            //string token = user.RefreshTokens.FirstOrDefault(x => x == refreshClaim.Value);

            //if (token != null)
            if (refreshClaim.Value == user.RefreshTokens)
            {
                var refreshToken = _tokenService.GenerateRefreshToken(user);

                user.RefreshTokens = refreshToken.refreshToken;

                //user.RefreshTokens.Remove(token);

                _users.Update(user);

                return new Tokens
                {
                    AccessToken = _tokenService.GenerateAccessToken(user),
                    RefreshToken = refreshToken.jwt
                };
            }
            else
            {
                throw new System.Exception("Refresh token incorrect");
            }
        }
    }
}