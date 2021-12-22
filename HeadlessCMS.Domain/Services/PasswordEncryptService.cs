using HeadlessCMS.Domain.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace HeadlessCMS.Domain.Services
{
    public class PasswordEncryptService : IPasswordEncryptService
    {
        private const int _length = 32;
        private const int _iterCount = 10000;

        public (string, byte[]) HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: _iterCount,
                numBytesRequested: _length));

            return (hashedPassword, salt);
        }

        public bool VerifyHashedPassword(string hashedPassword, byte[] salt, string providedPassword)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                 password: providedPassword,
                 salt: salt,
                 prf: KeyDerivationPrf.HMACSHA256,
                 iterationCount: _iterCount,
                 numBytesRequested: _length));

            return hashed.SequenceEqual(hashedPassword);
        }
    }
}