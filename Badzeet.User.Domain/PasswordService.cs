using System;
using System.Security.Cryptography;

namespace Badzeet.User.Domain
{
    internal class PasswordService
    {
        private const int iterations = 10000;
        internal string GetHashedPassword(string password)
        {
            var salt = GetSalt();
            var hashBytes = new byte[36];
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                var hash = pbkdf2.GetBytes(20);
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);
            }

            return Convert.ToBase64String(hashBytes);
        }

        internal static byte[] GetSalt()
        {
            var salt = new byte[16];
            using (var cryptoServiceProvider = new RNGCryptoServiceProvider())
                cryptoServiceProvider.GetBytes(salt);
            return salt;
        }

        internal bool Verify(string savedPasswordHash, string password)
        {
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }
    }
}