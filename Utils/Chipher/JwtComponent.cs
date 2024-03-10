using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace BurgerApi.Utils.Chipher
{
    public class JwtComponent
    {
        private IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        public string EncryptData(string userId, string userEmail)
        {
            var tokenHdlr = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, userEmail),
                new(JwtRegisteredClaimNames.Email, userEmail),
                new("userId", userId),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = config["JwtSettings:Issuer"],
                Audience = config["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHdlr.CreateToken(tokenDescriptor);

            var jwt = tokenHdlr.WriteToken(token);
            return jwt;
        }
    }
}
