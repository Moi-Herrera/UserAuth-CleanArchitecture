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
    //implementacion de los metodos definidos en IuserRepository
    public class UserRepository : IuserRepository
    {
        //contexto de la base de datos
        private readonly AppDbContext _context;

        //inyeccion de dependencia del contexto de la base de datos
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
     
        //obtener usuario por email incluyendo sus refresh tokens
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Email == email);
        }
        //agregar un nuevo usuario
        public async Task AddAsync(User user)
        {
           await _context.Users.AddAsync(user);
        }
        //actualizar un usuario existente
        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
        }
        //guardar los cambios realizados enla base de datos
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
