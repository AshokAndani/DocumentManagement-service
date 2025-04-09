using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DocumentManagement.Services
{

    /// <summary>
    /// Provides JWT token related functionalities.
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        /// Generates the JWT token.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="role"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        string GenerateToken(string username, string role, int userId);
    }

    /// <summary>
    /// Provides JWT token related functionalities.
    /// </summary>
    /// <param name="Configuration"></param>
    public class JwtTokenService(IConfiguration Configuration) : IJwtTokenService
    
    {
        /// <summary>
        /// Generates the JWT token.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="role"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public string GenerateToken(string username, string role, int userId)
        {
            string secretKey = Configuration["JwtOptions:SecretKey"] ?? throw new ArgumentNullException("cannot be null");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
