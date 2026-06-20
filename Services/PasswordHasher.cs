using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace water_shop.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            using var Argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
            Argon2.Salt = salt;
            Argon2.DegreeOfParallelism = 8;
            Argon2.Iterations = 4;
            Argon2.MemorySize = 65536;
            byte[] hash = Argon2.GetBytes(32);
            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
            ;
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var parts = hashedPassword.Split('.');
            if (parts.Length != 2) return false;
            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] stroredHash = Convert.FromBase64String(parts[1]);

            using var Argon2 = new Argon2id(Encoding.UTF8.GetBytes(providedPassword));
            Argon2.Salt = salt;
            Argon2.DegreeOfParallelism = 8;
            Argon2.Iterations = 4;
            Argon2.MemorySize = 65536;
            byte[] computedHash = Argon2.GetBytes(32);
            return CryptographicOperations.FixedTimeEquals(stroredHash, computedHash);
        }
    }
}
