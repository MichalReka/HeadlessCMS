using HeadlessCMS.ApplicationCore.Core.Interfaces.Services;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HeadlessCMS.ApplicationCore.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly DbSet<User> _users;
        private readonly ITokenService _tokenService;
        private readonly IApplicationDbContext _dbContext;
        private readonly IPasswordEncryptService _passwordEncryptService;

        public UserAuthService(IApplicationDbContext dbContext, ITokenService tokenService, IPasswordEncryptService passwordEncryptService)
        {
            _users=dbContext.Set<User>();
            _tokenService=tokenService;
            _dbContext=dbContext;
            _passwordEncryptService = passwordEncryptService;
        }

        public async Task<Tokens> LoginAsync(Authentication authentication)
        {

            User user = _users.FirstOrDefault(u => u.Name == authentication.Name);

            if (user == null)
                throw new Exception("User doesn't exist");

            bool validPassword = _passwordEncryptService.VerifyHashedPassword(user.Password,authentication.Password);

            if (validPassword)
            {
                var refreshToken = _tokenService.GenerateRefreshToken(user);

                //if (user.RefreshTokens == null)
                //    user.RefreshTokens = new List<string>();

                user.RefreshTokens = refreshToken.jwt;

                _users.Update(user);
                await _dbContext.SaveChangesAsync();

                return new Tokens
                {
                    AccessToken = _tokenService.GenerateAccessToken(user),
                    RefreshToken = refreshToken.jwt
                };
            }
            else
            {
                throw new Exception("Username or password incorrect");
            }
        }

        public async Task<Tokens> RefreshAsync(Claim userClaim, Claim refreshClaim)
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
                await _dbContext.SaveChangesAsync();

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