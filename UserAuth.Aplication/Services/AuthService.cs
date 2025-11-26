using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserAuth.Aplication.DTOs;
using UserAuth.Aplication.Interfaces;
using UserAuth.Domain.Entities;

namespace UserAuth.Aplication.Services
{
    public class AuthService
    {
        private readonly IuserRepository _userRepo;
        private readonly IRefreshTokenRepository _tokenRepo;
        private readonly PasswordService _passwordService;
        private readonly IConfiguration _config;

        public AuthService(
            IuserRepository userRepo,
            IRefreshTokenRepository tokenRepo,
            PasswordService passwordService,
            IConfiguration config)
        {
            _userRepo = userRepo;
            _tokenRepo = tokenRepo;
            _passwordService = passwordService;
            _config = config;
        }

        // ============================
        //       REGISTRO
        // ============================
        public async Task<AuthResponseDto> RegisterAsync(UserCreateDto dto)
        {
            var existing = await _userRepo.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new Exception("El usuario ya existe");

            // Generar hash + salt
            _passwordService.CreatePasswordHash(dto.Password, out var hash, out var salt);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();

            return await GenerateTokens(user);
        }

        // ============================
        //         LOGIN
        // ============================
        public async Task<AuthResponseDto> LoginAsync(UserLoginDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("Credenciales incorrectas");

            var valid = _passwordService.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt);
            if (!valid)
                throw new Exception("Credenciales incorrectas");

            return await GenerateTokens(user);
        }

        // ============================
        //      GENERAR TOKENS
        // ============================
        private async Task<AuthResponseDto> GenerateTokens(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim("username", user.Username),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256)
            };

            var jwt = tokenHandler.CreateToken(descriptor);
            var accessToken = tokenHandler.WriteToken(jwt);

            // Revocar tokens antiguos
            var oldTokens = await _tokenRepo.GetUserTokensAsync(user.Id);
            foreach (var t in oldTokens)
            {
                t.RevokedAt = DateTime.UtcNow;
                _tokenRepo.Update(t);
            }

            // Crear nuevo refresh token
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString("N"),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _tokenRepo.AddAsync(refreshToken);
            await _tokenRepo.SaveChangesAsync();

            return new AuthResponseDto
            {
                Username = user.Username,
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }

        // ============================
        //      REFRESCAR TOKEN
        // ============================
        public async Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken)
        {
            var token = await _tokenRepo.GetByTokenAsync(refreshToken);
            if (token == null)
                return null;

            if (token.RevokedAt != null)
                return null;

            if (token.ExpiresAt < DateTime.UtcNow)
                return null;

            var user = await _userRepo.GetByIdAsync(token.UserId);
            if (user == null)
                return null;

            // Revocar el token viejo
            token.RevokedAt = DateTime.UtcNow;
            _tokenRepo.Update(token);
            await _tokenRepo.SaveChangesAsync();

            // Generar nuevos tokens
            return await GenerateTokens(user);
        }

        // ============================
        //         LOGOUT
        // ============================
        public async Task LogoutAsync(int userId)
        {
            // 1. Obtener todos los tokens del usuario
            var tokens = await _tokenRepo.GetUserTokensAsync(userId);

            // 2. Revocar todos los tokens
            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
                token.RevokedReason = "User logout";
            }

            // 3. Guardar cambios
            await _tokenRepo.SaveChangesAsync();
        }
    }
}
