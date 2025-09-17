using MeetUp.EShop.Presentation.Models.Order;
using MeetUp.EShop.Presentation.Models.User;
using Refit;

namespace MeetUp.EShop.Tests.RefitClients
{
    public interface IUserAPI
    {
        [Put("/api/User/update")]
        Task<ApiResponse<Guid>> UpdateUser(UpdateUserData user);

        [Get("/api/User/getUser")]
        Task<ApiResponse<RegisterUserUI>> GetUser(Guid id);

        [Post("/api/User/addProductToOrder")]
        Task<ApiResponse<bool>> AddProductToOrder(Guid productId, Guid userId);

        [Get("/api/User/getCart")]
        Task<ApiResponse<List<Guid>>> GetCart(Guid userId);

        [Get("/api/User/getLastOrder")]
        Task<ApiResponse<Order>> GetLastOrder(Guid userId);
        Task<ApiResponse<object>> DeleteUser(Guid userId);
    }
}
