using Refit;
using MeetUp.EShop.Presentation.Models.Product;

namespace MeetUp.EShop.Presentation.Services.Inteerfaces
{
    public interface IProductAPI
    {
        [Get("/api/Product/GetProducts")]
        Task<ApiResponse<List<Models.Product.Product>>> GetProducts();
    }
}
