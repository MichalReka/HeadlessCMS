using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Domain.Interfaces;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Configuration;
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
            return new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(Encoding.UTF8.GetBytes(_secret))
                .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds())
                .AddClaim("username", user.Name)
                .Issuer(@"https://localhost:44316")
                .Audience(@"https://localhost:44316")
                .Encode();
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

            return (randomString, jwt);
        }
    }
}