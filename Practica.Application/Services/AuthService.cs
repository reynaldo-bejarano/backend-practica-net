using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Practica.Application.Interfaces;
using Practica.Domain.Entities;
using Practica.Infrastructure.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Practica.Application.Services
{
    public class AuthService : IAuthService
    {

        private readonly AppDBContext _context;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public AuthService(AppDBContext context, IConfiguration configuration)
        {
           _context = context;
            _secretKey = Environment.GetEnvironmentVariable("JWT_SECRET");
            _issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            _audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            return GenerateToken(user); // Generar y devolver el token
        }



        /// <summary>
        /// Gerarate Token JWT
        /// </summary>
        /// <returns></returns>
        private string GenerateToken(User user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token); 
        }

        /// <summary>
        /// Verify password and hashed password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        private bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
