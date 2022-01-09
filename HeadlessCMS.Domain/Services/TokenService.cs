using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Domain.Interfaces;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HeadlessCMS.Domain.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _secret;

        public TokenService(IConfiguration configuration)
        {
            _secret = configuration.GetSection("Token").Value;
        }

        public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(2);

            var token = new JwtSecurityToken(
                "https://localhost:44316",
                "https://localhost:44316",
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public (string refreshToken, string jwt) GenerateRefreshToken(User user)
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                Convert.ToBase64String(randomNumber);
            }

            var randomString = System.Text.Encoding.UTF8.GetString(randomNumber);

            string jwt = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(_secret)
                .AddClaim("exp", DateTimeOffset.UtcNow.AddDays(2).ToUnixTimeSeconds())
                .AddClaim("refresh", randomString)
                .AddClaim("username", user.Name)
                .Issuer(@"https://localhost:44316")
                .Audience(@"https://localhost:44316")
                .Encode();


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(2);
            //var token = new JwtSecurityToken(
            //    "https://localhost:44316",
            //    "https://localhost:44316",
            //    claims,
            //    expires: expires,
            //    signingCredentials: creds
            //);

            //return new JwtSecurityTokenHandler().WriteToken(token);

            return (randomString, jwt);
        }
    }
}