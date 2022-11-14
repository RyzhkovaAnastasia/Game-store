using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineGameStore.BLL.Models.AuthModels;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OnlineGameStore.BLL.Configurations.JWT
{
    public class JWT
    {
        private readonly IOptions<AuthOptions> _authOptions;

        public JWT(IOptions<AuthOptions> authOptions)
        {
            _authOptions = authOptions;
        }
        public string GenerateJWT(UserModel user)
        {
            var authOptions = _authOptions.Value;

            var securityKey = authOptions.SymmetricSecurityKey;
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              authOptions.Issuer,
              authOptions.Audience,
              new[]
              {
                  new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                  new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                  new Claim(JwtRegisteredClaimNames.Email, user.Email),
                  new Claim(ClaimTypes.Role, user.Role ?? "Guest"),
                  new Claim("role", user.Role ?? "Guest"),
                  new Claim("publisherId", user.PublisherId.HasValue ? user.PublisherId.ToString() : "")
              },
              expires: DateTime.Now.AddMinutes(authOptions.TokenLifetime),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
