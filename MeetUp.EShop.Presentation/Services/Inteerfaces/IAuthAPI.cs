using MeetUp.EShop.Presentation.Models.Token;
using MeetUp.EShop.Presentation.Models.User;
using Refit;

namespace MeetUp.EShop.Presentation.Services.Inteerfaces
{
    public interface IAuthAPI
    {
        [Post("/api/Auth/Login")]
        Task<ApiResponse<AccessToken>> Login(LoginUser user);

        [Post("/api/User/Register")]
        Task<ApiResponse<Guid>> Register(RegisterUser user);
    }
}
