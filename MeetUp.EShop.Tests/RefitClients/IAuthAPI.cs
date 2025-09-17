using MeetUp.EShop.Presentation.Models.Token;
using MeetUp.EShop.Presentation.Models.User;
using Refit;

namespace MeetUp.EShop.Tests.RefitClients
{
    public interface IAuthAPI
    {
        [Post("/api/Auth/Login")]
        Task<ApiResponse<AccessToken>> Login(LoginUser user);

        [Post("/api/User/Register")]
        Task<ApiResponse<Guid>> Register(RegisterUserUI user);
    }
}
