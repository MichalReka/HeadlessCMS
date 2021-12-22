using HeadlessCMS.ApplicationCore.Core.Interfaces.Services;
using HeadlessCMS.ApplicationCore.Dtos;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Domain.Interfaces;
using HeadlessCMS.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AutoMapper;


namespace HeadlessCMS.ApplicationCore.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly DbSet<User> _users;
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IPasswordEncryptService _passwordEncryptService;
        private readonly IMapper _mapper;

        public UserAuthService(ApplicationDbContext dbContext, ITokenService tokenService, IPasswordEncryptService passwordEncryptService, IMapper mapper)
        {
            _users=dbContext.Set<User>();
            _tokenService=tokenService;
            _dbContext=dbContext;
            _passwordEncryptService = passwordEncryptService;
            _mapper = mapper;
        }

        public User HandleRegister(UserDto userDto)
        {
            User user = _mapper.Map<User>(userDto);
            (user.Password, user.Salt) = _passwordEncryptService.HashPassword(user.Password);
            return user;
        }

        public async Task<Tokens> LoginAsync(Authentication authentication)
        {

            User user = _users.FirstOrDefault(u => u.Name == authentication.Name);

            if (user == null)
                throw new Exception("User doesn't exist");

            bool validPassword = _passwordEncryptService.VerifyHashedPassword(user.Password, user.Salt, authentication.Password);

            if (validPassword)
            {
                var refreshToken = _tokenService.GenerateRefreshToken(user);

                //if (user.RefreshTokens == null)
                //    user.RefreshTokens = new List<string>();

                user.RefreshTokens = refreshToken.refreshToken;

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