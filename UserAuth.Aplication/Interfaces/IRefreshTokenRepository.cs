using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UserAuth.Domain.Entities;

namespace UserAuth.Aplication.Interfaces
{
    //definicion de los metodos para el repositorio de RefreshToken
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task AddAsync(RefreshToken refreshToken);// agregar refresh token
        Task<bool> SaveChangesAsync();//guardar cambios
        void Update(RefreshToken token);
        Task<IEnumerable<RefreshToken>> GetUserTokensAsync(int userId);
    }
}
