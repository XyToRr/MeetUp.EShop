using MeetUp.EShop.Core.Enums;
using MeetUp.EShop.Core.Interfaces;
using MeetUp.EShop.Core.Models.Product;
using MeetUp.EShop.Core.Models.Token;
using MeetUp.EShop.Core.Models.User;
using MeetUp.EShop.Core.Models.Order;


namespace MeetUp.EShop.Business.Services
{
    public class UserService(IUserRepository userRepository, OrderService orderService, ProductService productService)
    {
        public async Task<Guid> Register(User user)
        {
            if (user.Login == string.Empty)
                return Guid.Empty;

            if (user.Password == string.Empty)
                return Guid.Empty;

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            return await userRepository.Register(user);
        }

        public IEnumerable<User> GetUsers() => userRepository.GetUsers();

        public User? Get(Guid id) => userRepository.Get(id);

        public Guid? GetByName(string name) => userRepository.GetByName(name);

        public async Task<bool> Update(User user) => await userRepository.Update(user);
        public async Task<bool> UpdateTokens(User user) => await userRepository.UpdateTokens(user);

        public Guid GetByRefreshToken(string refreshToken)
        {
            return userRepository.GetUsers().FirstOrDefault(u => u.RefreshToken == refreshToken)?.Id ?? Guid.Empty;
        }

        public async Task AddProductToOrder(Product product, User user)
        {
            var lastOrder = user.Orders.LastOrDefault(o => o.Status == OrderStatus.New);
            if (lastOrder != null)
            {
                lastOrder.Products.Add(product);
                lastOrder.TotalPrice += product.Price;
                await orderService.UpdateOrder(lastOrder);
            }
            else
            {
                var newOrderNumber = 1;
                if (user.Orders != null && user.Orders.Count > 0)
                {
                    newOrderNumber = user.Orders.Count + 1;
                }
                lastOrder = new Order
                {
                    Id = Guid.NewGuid(),
                    Number = user.Orders.Count + 1,
                    TotalPrice = product.Price,
                    Status = OrderStatus.New,
                    CreatedAt = DateTime.UtcNow,
                    UserId = user.Id,
                    Products = new List<Product> { product }
                };
                await orderService.AddOrder(lastOrder);
                
                if (product.Orders == null)
                    product.Orders = new List<Order>();
                product.Orders.Add(lastOrder);
                
                await productService.UpdateProduct(product);

                if(user.Orders == null)
                    user.Orders = new List<Order>();
                user.Orders.Add(lastOrder);
                await Update(user);

            }

        }
    }
}
