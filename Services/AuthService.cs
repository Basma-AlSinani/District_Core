using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using CrimeManagment.Models;
using CrimeManagment.Services;
using CrimeManagment.DTOs;
using static CrimeManagement.DTOs.AuthDtos;

namespace CrimeManagment.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUsersService _usersService;

        public AuthService(IConfiguration config, IUsersService usersService)
        {
            _config = config;
            _usersService = usersService;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDtos dto)
        {
            var user = await _usersService.AuthenticateAsync(dto.Email, dto.Password);
            if (user == null) return null;

            var token = GenerateToken(user);
            return new LoginResponseDto
            {
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = token
            };
        }

        private string GenerateToken(Users user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(6),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
