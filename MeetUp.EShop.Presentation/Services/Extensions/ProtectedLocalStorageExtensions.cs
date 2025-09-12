using MeetUp.EShop.Presentation.Models.Token;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace MeetUp.EShop.Presentation.Services.Extensions
{
    public static class ProtectedLocalStorageExtensions
    {
        public static async Task<AccessToken> GetTokenAsync(this ProtectedLocalStorage localStorage)
        {
            var result = await localStorage.GetAsync<AccessToken>("token");
            var token = result.Success ? result.Value : null;

            return token;
        }

        public static async Task SetTokenAsync(this ProtectedLocalStorage localStorage, AccessToken token)
        {
            await localStorage.SetAsync("token", token);
        }

        public static async Task RemoveTokenAsync(this ProtectedLocalStorage localStorage)
        {
            await localStorage.DeleteAsync("token");
        }
        
        public static async Task<ClaimsPrincipal> GetTokenClaimsAsync(this ProtectedLocalStorage localStorage)
        {
            var token = await localStorage.GetTokenAsync();
            if (token == null || string.IsNullOrEmpty(token.JWTToken))
            {
                return new ClaimsPrincipal(new ClaimsIdentity());
            }
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token.JWTToken);
            var claimsIdentity = new ClaimsIdentity(jwtToken.Claims, "jwt");
            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}
