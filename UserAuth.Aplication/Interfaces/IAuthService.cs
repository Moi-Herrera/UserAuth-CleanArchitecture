using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuth.Aplication.DTOs;

namespace UserAuth.Aplication.Interfaces
{
    public interface IAuthService
    {
        //iniciar sesión y refrescar token
        Task<AuthResponseDto> RegisterAsync(string username, string email, string password);
        Task<AuthResponseDto> LoginAsync(string email, string password);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(int userId);
    }
}
