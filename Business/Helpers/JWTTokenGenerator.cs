using MeetUp.EShop.Business.Interfaces;
using MeetUp.EShop.Core.Models.Token;
using MeetUp.EShop.Core.Models.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MeetUp.EShop.Business.Helpers
{
    public class AccessTokenGenerator(IConfiguration configuration) : ITokenGenerator
    {
        public AccessToken GenerateToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(configuration["Token"]);


            var securityKey = new SymmetricSecurityKey(key);

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: new[]
                { 
                    new Claim("ID", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Login) ,
                    new Claim(ClaimTypes.Email, user.Email),
                },
                expires: DateTime.UtcNow.AddSeconds(300),
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.WriteToken(token);
            var refreshToken = Guid.NewGuid().ToString();
            
            var accessToken = new AccessToken
            {
                JWTToken = jwt,
                RefreshToken = refreshToken,
                RefreshTokenExpire = DateTime.UtcNow.AddHours(10)
            };
            return accessToken;

        }
    }
}
