using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerceWebSite.Dto;
using Microsoft.IdentityModel.Tokens;

namespace ECommerceWebSite.Utilitys
{
    public class GenerateJWT
    {
        private IConfiguration _configuration;

        public GenerateJWT(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(UserToListDto user)
        {
            // Retrieve the issuer audience and key which we mwntioned in the app settings using the IConfiguration(where IConfiguratuon is a predefined interface which we used to take strings from appsettings)
            var issuer = _configuration["JWTConfig:Issuer"];
            var audience = _configuration["JWTConfig:Audience"];
            var key = _configuration["JWTConfig:Key"];
            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWTConfiguration settings are missing or invalid");
            }

            // Create claims for the token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sid,user.idOfUser.ToString()),
            };

            // Configure token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.UtcNow.AddHours(10),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            // Generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);


        }
    }
}
