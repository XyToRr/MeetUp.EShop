using System.Net;
using MeetUp.EShop.Api.Cache;
using MeetUp.EShop.Api.Exceptions;
using MeetUp.EShop.Business.Cache.Interfaces;
using MeetUp.EShop.Business.Services;
using MeetUp.EShop.Core.Enums;
using MeetUp.EShop.Core.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MeetUp.EShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ProductService _productService;
        private readonly ICacheService _cacheService;

        public UserController(UserService userService, ProductService productService, ICacheService cache)
        {
            _userService = userService;
            _productService = productService;
            _cacheService = cache;
        }

        [HttpPost("register")]

        public async Task<IResult> Register(RegisterUser user)
        {
            var result = await _userService.Register(user);
            if (result == Guid.Empty)
            {
                throw new ControllerException("bad register data", HttpStatusCode.BadRequest);
            }

            var userData = _userService.Get(result);
            await _cacheService.SetCacheAsync($"{CacheKeys.SingleUser}+{result}", userData);
            await _cacheService.SetCacheAsync(CacheKeys.Users, _userService.GetUsers().ToList());

            Log.Information("Successfully registered user with ID {UserId}", result);
            return Results.Ok(result);
        }

        [HttpDelete("delete")]
        [Authorize]
        public async Task<IResult> DeleteUser([FromBody] Guid id)
        {
            var user = _userService.Get(id);
            if (user == null)
            {
                throw new ControllerException($"not found user with id: {id}", HttpStatusCode.NotFound);
            }
            var result = await _userService.Delete(id);
            if (!result)
            {
                throw new ControllerException("bad delete data", HttpStatusCode.BadRequest);
            }

            await _cacheService.RemoveCacheAsync($"{CacheKeys.SingleUser}+{id}");
            await _cacheService.SetCacheAsync(CacheKeys.Users, _userService.GetUsers().ToList());

            Log.Information("Successfully deleted user with ID {UserId}", id);
            return Results.Ok();
        }

        [HttpGet("getUsers")]
        public async Task<IResult> GetUsers()
        {
            var usersCache = await _cacheService.GetCacheAsync<List<User>>(CacheKeys.Users);
            if (usersCache != null)
            {
                return Results.Ok(usersCache);
            }

            var users = _userService.GetUsers();
            if (users == null || !users.Any())
            {
                throw new ControllerException("not found users", HttpStatusCode.NotFound);
            }

            await _cacheService.SetCacheAsync(CacheKeys.Users, users.ToList());

            Log.Information("Retrieved {Count} users successfully", users.Count());
            return Results.Ok(users);
        }

        [HttpGet("getUser")]
        public async Task<IResult> GetUser([FromBody] Guid id)
        {
            var userCacheKey = $"{CacheKeys.SingleUser}+{id}";
            var userCache = await _cacheService.GetCacheAsync<User>(userCacheKey);
            if (userCache != null)
            {
                return Results.Ok(userCache);
            }

            var user = _userService.Get(id);
            if (user == null)
            {
                throw new ControllerException($"not found user with id: {id}", HttpStatusCode.NotFound);
            }

            await _cacheService.SetCacheAsync(userCacheKey, user);

            Log.Information("Retrieved user with ID {UserId} successfully", id);
            return Results.Ok(user);
        }

        [HttpGet("getUserByName")]
        public IResult GetUserByName(string name)
        {
            var user = _userService.GetByName(name);
            if (user == null)
            {
                throw new ControllerException($"not found user with name: {name}", HttpStatusCode.NotFound);
            }

            Log.Information("Retrieved user with name {UserName} successfully", name);
            return Results.Ok(user);
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IResult> UpdateUser([FromBody] UpdateUser user)
        {
            var updateUser = new UpdateUser
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Login = user.Login,
                Password = user.Password
            };
            var result = await _userService.Update(updateUser);
            if (!result)
            {
                throw new ControllerException("bad update data", HttpStatusCode.BadRequest);
            }

            var userCacheKey = $"{CacheKeys.SingleUser}+{user.Id}";
            await _cacheService.SetCacheAsync(userCacheKey, _userService.Get(user.Id));
            await _cacheService.SetCacheAsync(CacheKeys.Users, _userService.GetUsers().ToList());

            Log.Information("Successfully updated user with ID {UserId}", user.Id);
            return Results.Ok(user.Id);
        }


        [HttpPost("addProductToOrder")]
        [Authorize]
        public async Task<IResult> AddProductToOrder(Guid productId, Guid userId)
        {
            var product = _productService.GetProduct(productId);
            if (product == null)
            {
                throw new ControllerException($"not found product with id: {productId}", HttpStatusCode.NotFound);
            }
            var user = _userService.Get(userId);
            if (user == null)
            {
                throw new ControllerException($"not found user with id: {userId}", HttpStatusCode.NotFound);
            }

            await _userService.AddProductToOrder(product, user);
            var cartCacheKey = $"{CacheKeys.UserCart}_{userId}";
            
            await _cacheService.RemoveCacheAsync(cartCacheKey);
            await _cacheService.RemoveCacheAsync($"{CacheKeys.UserLastOrder}_{userId}");

            return Results.Ok();
        }

        [HttpGet("getCart")]
        [Authorize]
        public async Task<IResult> GetCart(Guid userId)
        { 
            var user = _userService.Get(userId);
            if (user == null)
            {
                throw new ControllerException($"not found user with id: {userId}", HttpStatusCode.NotFound);
            }

            var cacheKey = $"{CacheKeys.UserCart}_{userId}";
            var cartCache = await _cacheService.GetCacheAsync<IEnumerable<Guid>>(cacheKey);
            if (cartCache != null)
            {
                return Results.Ok(cartCache);
            }

            var cart = user.Orders.LastOrDefault(o => o.Status == OrderStatus.New)?.Products.Select(p=>p.Id);
            
            await _cacheService.SetCacheAsync(cacheKey, cart?.ToList() ?? new List<Guid>());
            return Results.Ok(cart);
        }

        [HttpGet("getLastOrder")]
        [Authorize]
        public async Task<IResult> GetLastOrder(Guid userId)
        {
            var cacheKey = $"{CacheKeys.UserLastOrder}_{userId}";
            var orderCache = await _cacheService.GetCacheAsync<object>(cacheKey);
            if (orderCache != null)
            {
                return Results.Ok(orderCache);
            }

            var user = _userService.Get(userId);
            if (user == null)
            {
                throw new ControllerException($"not found user with id: {userId}", HttpStatusCode.NotFound);
            }
            var lastOrder = user.Orders.LastOrDefault(o => o.Status == OrderStatus.New);
            if (lastOrder == null)
            {
                throw new ControllerException($"not found last order for user with id: {userId}", HttpStatusCode.NotFound);
            }

            await _cacheService.SetCacheAsync(cacheKey, lastOrder);
            return Results.Ok(lastOrder);
        }
    }
}
