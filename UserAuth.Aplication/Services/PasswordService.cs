using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UserAuth.Aplication.Services
{
    public class PasswordService
    {
        //metodo para crear hash y salt de la contraseña
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //usar HMACSHA512 para crear hash y salt
            using var hmac = new HMACSHA512();
            
            passwordSalt = hmac.Key;//generar salt unico
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));//crear hash
            
        }

        //metodo para verificar la contraseña
        public bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);//comparar hash
            
        }

        public byte[] GenerateSalt()
        {
            var salt = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }

        public byte[] Hashpassword(string password, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
