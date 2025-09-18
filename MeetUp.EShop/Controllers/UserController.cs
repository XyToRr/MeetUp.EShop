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
        private readonly IHybridCacheService _hybridCacheService;

        public UserController(
            UserService userService, 
            ProductService productService, 
            IHybridCacheService hybridCache)
        {
            _userService = userService;
            _productService = productService;
           _hybridCacheService = hybridCache;
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
            
            await _hybridCacheService.SetCacheAsync(CacheKeys.Users, _userService.GetUsers().ToList());

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

            await _hybridCacheService.RemoveCacheAsync($"{CacheKeys.SingleUser}{id}");
            await _hybridCacheService.SetCacheAsync(CacheKeys.Users, _userService.GetUsers().ToList());

            Log.Information("Successfully deleted user with ID {UserId}", id);
            return Results.Ok();
        }

        [HttpGet("getUsers")]
        public async Task<IResult> GetUsers()
        {
            var users = await _hybridCacheService.GetCacheAsync(CacheKeys.Users,
                async () => await Task.FromResult(_userService.GetUsers()));

            if (users == null || !users.Any())
            {
                throw new ControllerException("not found users", HttpStatusCode.NotFound);
            }

            return Results.Ok(users);

        }

        [HttpGet("getUser")]
        public async Task<IResult> GetUser([FromBody] Guid id)
        {
          
            var user = _hybridCacheService.GetCacheAsync($"{CacheKeys.SingleUser}{id}",
                async () => await Task.FromResult(_userService.Get(id)));
            
            if (user == null)
            {
                throw new ControllerException($"not found user with id: {id}", HttpStatusCode.NotFound);
            }

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

            await _hybridCacheService.SetCacheAsync($"{CacheKeys.SingleUser}{user.Id}", _userService.Get(user.Id));
            await _hybridCacheService.SetCacheAsync(CacheKeys.Users, _userService.GetUsers().ToList());

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

            await _hybridCacheService.RemoveCacheAsync($"{CacheKeys.UserLastOrder}{userId}");
            await _hybridCacheService.RemoveCacheAsync($"{CacheKeys.SingleUser}{userId}");
            await _hybridCacheService.RemoveCacheAsync(CacheKeys.Orders);


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

            var cart = user.Orders.LastOrDefault(o => o.Status == OrderStatus.New)?.Products.Select(p=>p.Id);
            //var cart = await _hybridCacheService.GetCacheAsync($"{CacheKeys.UserCart}{userId}",
            //    async () => await Task.FromResult(user.Orders.LastOrDefault(o => o.Status == OrderStatus.New)?.Products.Select(p=>p.Id)));
         
            return Results.Ok(cart);
        }

        [HttpGet("getLastOrder")]
        [Authorize]
        public async Task<IResult> GetLastOrder(Guid userId)
        {
            var order = await _hybridCacheService.GetCacheAsync($"{CacheKeys.UserLastOrder}{userId}",
                async () =>
                {
                    var user = _userService.Get(userId);
                    return await Task.FromResult(user?.Orders.LastOrDefault(o => o.Status == OrderStatus.New));
                });

         
            if (order == null)
            {
                throw new ControllerException($"not found last order for user with id: {userId}", HttpStatusCode.NotFound);
            }

            return Results.Ok(order);
        }
    }
}
