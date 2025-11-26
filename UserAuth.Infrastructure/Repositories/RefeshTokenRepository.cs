using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuth.Aplication.Interfaces;
using UserAuth.Domain.Entities;
using UserAuth.Infrastructure.Data;

namespace UserAuth.Infrastructure.Repositories
{
    //implementacion de los metodos definidos en IRefreshTokenRepository
    public class RefeshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        //inyeccion de dependencia del contexto de la base de datos
        public RefeshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        //agregar un nuevo refresh token
        public async Task AddAsync(RefreshToken refreshToken)
        {
           await _context.RefreshTokens.AddAsync(refreshToken);
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
              .FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task<IEnumerable<RefreshToken>> GetUserTokensAsync(int userId)
        {
            return await _context.RefreshTokens
            .Where(x => x.UserId == userId)
            .ToListAsync();
        }

        //guardar los cambios realizados enla base de datos
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(RefreshToken token)
        {
            throw new NotImplementedException();
        }
    }
}
