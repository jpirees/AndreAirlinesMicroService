using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Authentications.API.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.Entities.DTO;

namespace Authentications.API.Services
{
    public class TokenService
    {
        public static string GenerateToken(UserResponseDTO user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(Settings.Secret);

            var role = user.Role.Description;

            var permissions = user.Role.Access.Select(access => new Claim(ClaimTypes.Role,  $"{role}_{access.Description}"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(permissions),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}
