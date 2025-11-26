using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuth.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        //Password protegida no se guardara en string
        //Password encriptada con HMAC-SHA512
        public Byte[] PasswordHash { get; set; } = Array.Empty<Byte>();
        public Byte[] PasswordSalt { get; set; } = Array.Empty<Byte>();

        //Fecha de creacion de cuenta
        public DateTime CreatedAt {  get; set; } = DateTime.UtcNow;

        //relacion 1 a muchos con RefreshToken
        //cada usuario puede tener mas de un RefreshToken
        public List<RefreshToken> RefreshTokens { get; set; } = new();
    }
}
