using Microsoft.AspNetCore.Components.Authorization;

namespace MeetUp.EShop.Presentation.Services.Extensions
{
    public static class AuthStateProviderExtensions
    {
        public static async Task<Guid> GetCurrentUserId(this AuthenticationStateProvider authStateProvider)
        {
            var state = await authStateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            var idClaim = user.Claims.FirstOrDefault(c => c.Type == "ID");
            if (idClaim != null)
            {
                return Guid.Parse(idClaim.Value);
            }
            return Guid.Empty;
        }
    }
}
