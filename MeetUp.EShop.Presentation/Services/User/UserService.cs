using MeetUp.EShop.Presentation.Models.Product;
using MeetUp.EShop.Presentation.Models.User;
using MeetUp.EShop.Presentation.Services.Inteerfaces;
using Product = MeetUp.EShop.Presentation.Models.Product.Product;

namespace MeetUp.EShop.Presentation.Services
{
    public class UserService
    {
        private readonly IUserAPI _userAPI;

        public UserService(IUserAPI userAPI)
        {
            _userAPI = userAPI;
        }

        public async Task<bool> UpdateUser(UpdateUserData user)
        {
            var response = await _userAPI.UpdateUser(user);
            return response.IsSuccessStatusCode;
        }

        public async Task<RegisterUser> GetUserById(Guid id)
        {
            var response = await _userAPI.GetUser(id);
            if (response.IsSuccessStatusCode)
            {
                return response.Content;
            }
            return null;
        }

        public async Task AddProductToOrder(Guid productId, Guid userId)
        {
            await _userAPI.AddProductToOrder(productId, userId);
        }
    }
}
