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

        public string Encrypt(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: _iterCount,
                numBytesRequested: _length));

            hashed = MixSaltWithHash(hashed, Convert.ToBase64String(salt));
            return hashed;
        }

        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            var salt = GetSaltFromEncodedPassword(hashedPassword);
            var convertedSalt = DecodeUrlBase64(salt);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                 password: providedPassword,
                 salt: convertedSalt,
                 prf: KeyDerivationPrf.HMACSHA256,
                 iterationCount: _iterCount,
                 numBytesRequested: _length));

            hashed = MixSaltWithHash(hashed, salt);
            return hashed.SequenceEqual(hashedPassword);
        }

        private string GetSaltFromEncodedPassword(string password)
        {
            var length = _length + 12;
            var salt = password.Substring(length/2, password.Length-_length);
            return salt;
        }

        private string MixSaltWithHash(string hash, string salt)
        {
            var firstHashHalf = hash.Substring(0, (int)(hash.Length / 2));
            var lastHashHalf = hash.Substring((int)(hash.Length / 2), (int)(hash.Length/2));

            return firstHashHalf + salt + lastHashHalf;
        }
    }
}