using MeetUp.EShop.Presentation.Services.Inteerfaces;
using MeetUp.EShop.Presentation.CircuitServicesAccesor;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MeetUp.EShop.Presentation.Services.Extensions;
using System.Net;
using MeetUp.EShop.Presentation.Services.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace MeetUp.EShop.Presentation.Handlers
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly IAuthAPI _authAPI;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly CircuitServicesAccesor.CircuitServicesAccesor _circuitServicesAccesor;

        public TokenHandler(IAuthAPI authAPI, CircuitServicesAccesor.CircuitServicesAccesor circuitServicesAccesor, AuthenticationStateProvider authenticationState)
        {
            _authAPI = authAPI;
            _circuitServicesAccesor = circuitServicesAccesor;
            _authStateProvider = authenticationState;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                await ConfigureRequest(request);
                var responce = await base.SendAsync(request, cancellationToken);
                if(responce.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if(_authStateProvider is EShopAuthStateProvider stateProvider)
                    {
                        await stateProvider.MakeAnonymous();
                    }
                }    
                return responce;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private async Task ConfigureRequest(HttpRequestMessage request)
        {
            var token = await _circuitServicesAccesor.Service.GetService<ProtectedLocalStorage>().GetTokenAsync();
            if (token == null)
                return;
            var accessToken = token.JWTToken;

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
    }
}
