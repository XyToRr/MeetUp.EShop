using MeetUp.EShop.Presentation.Models.Token;
using MeetUp.EShop.Presentation.Services.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MeetUp.EShop.Presentation.Services.Authorization
{
    public class EShopAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _localStorage;
        public EShopAuthStateProvider(ProtectedLocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetTokenAsync();
     
            if (token == null || string.IsNullOrEmpty(token.JWTToken))
            {
                await MakeAnonymous();
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token.JWTToken);
            var expirationTime = jwtToken.ValidTo;
            if (expirationTime < DateTime.UtcNow)
            {
                await MakeAnonymous();
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var user = await _localStorage.GetTokenClaimsAsync();
            return new AuthenticationState(user);
        }

        public async Task Login(AccessToken token)
        {
            await _localStorage.SetTokenAsync(token);
            var user = await _localStorage.GetTokenClaimsAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task MakeAnonymous()
        {
            await _localStorage.RemoveTokenAsync();
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }
    }
}
