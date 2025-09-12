using MeetUp.EShop.Presentation.Models.Order;
using Refit;

namespace MeetUp.EShop.Presentation.Services.Inteerfaces
{
    public interface IOrderAPI
    {
        [Put("/api/Order/updateOrder")]
        Task<ApiResponse<bool>> UpdateOrder(Order order);
    }
}
