using Microsoft.Extensions.Options;
using System;
using System.Security.Cryptography;
using System.Text;

namespace King.David.Consulting.Common.AspNetCore.Security.Password
{
    public class PasswordHasher : IPasswordHasher
    {
        public PasswordHasher(IOptions<PasswordHasherOptions> passwordHasherOptions)
        {
            _passwordHasherOptions = passwordHasherOptions.Value;
        }
        private PasswordHasherOptions _passwordHasherOptions;

        public byte[] Hash(string password, byte[] salt)
        {
            HMACSHA512 x = new HMACSHA512(Encoding.UTF8.GetBytes(_passwordHasherOptions.Key));
            var bytes = Encoding.UTF8.GetBytes(password);

            var allBytes = new byte[bytes.Length + salt.Length];
            Buffer.BlockCopy(bytes, 0, allBytes, 0, bytes.Length);
            Buffer.BlockCopy(salt, 0, allBytes, bytes.Length, salt.Length);

            return x.ComputeHash(allBytes);
        }
    }
}
