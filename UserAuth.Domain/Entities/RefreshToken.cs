using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuth.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }

        //refresh token generado
        public string Token { get; set; } = string.Empty;

        //fecha en la que expira
        public DateTime ExpiresAt { get; set; }
        //fecha de creacion
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //fecha en que se invalido 
        public DateTime RevokedAT { get; set; }

        //Relacion con User
        public int UserId { get; set; }//Fk del usuario
        public User? User { get; set; }//Navegacion para la relacion 
    }
}
