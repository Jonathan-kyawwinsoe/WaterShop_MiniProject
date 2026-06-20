using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace water_shop.Services
{
    public class JwtProvider: IJwtProvider
    {
        private readonly JwtOption _jwtOption;
        public JwtProvider(JwtOption jwtOption)
        {
            _jwtOption = jwtOption;
        }
        public JwtProvider(IOptions<JwtOption> jwtOption)
        {
            _jwtOption = jwtOption.Value;
        }
        public string GenerateToken(string adminId, string userName)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, adminId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName.ToString()),
                new Claim(ClaimTypes.Name, userName.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOption.Issuer,
                audience: _jwtOption.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOption.ExpiryMinutes),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    
}
