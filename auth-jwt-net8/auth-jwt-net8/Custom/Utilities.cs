using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using auth_jwt_net8.Models;

namespace auth_jwt_net8.Custom
{
    public class Utilities
    {
        
        private readonly IConfiguration _configuration;
        public Utilities(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string encryptSHA256(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                // Convert byte array to a string
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                // Return the hexadecimal string.
                return builder.ToString();
            }
        }
        public string generateJWT(User model){

            // Create a new list of claims
            var userClaims = new[]
            {
                // Si el usuario es autenticado, se le asigna un Claim con el nombre de usuario
                // Aquí se puede agregar cualquier otro Claim que se desee dependiendo de la información que se quiera almacenar en el token
                new Claim(ClaimTypes.NameIdentifier, model.UserId.ToString()),
                new Claim(ClaimTypes.Email, model.Email!),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // Create a new token
            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
            

        }
    }
}
