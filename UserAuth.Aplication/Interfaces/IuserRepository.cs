using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuth.Domain.Entities;

namespace UserAuth.Aplication.Interfaces
{
    //definicion de los metodos para el repositorio de User
    public interface IuserRepository
    {
        Task<User?> GetByIdAsync(int id);//obtener usuario por id
        Task<User?> GetByEmailAsync(string email);//obtener usuario por email
        Task AddAsync(User user);//agregar usuario
        Task UpdateAsync(User user);//actualizar usuario
        Task<bool> SaveChangesAsync();//guardar cambios
    }
}
