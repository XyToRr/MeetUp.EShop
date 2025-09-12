using System.Net;
using MeetUp.EShop.Api.Exceptions;
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

        public UserController(UserService userService, ProductService productService)
        {
            _userService = userService;
            _productService = productService;
        }

        [HttpPost("register")]

        public async Task<IResult> Register(User user)
        {
            var result = await _userService.Register(user);
            if (result == Guid.Empty)
            {
                throw new ControllerException("bad register data", HttpStatusCode.BadRequest);
            }

            Log.Information("Successfully registered user with ID {UserId}", result);
            return Results.Ok(result);
        }

        [HttpGet("getUsers")]
        public IResult GetUsers()
        {
            var users = _userService.GetUsers();
            if (users == null || !users.Any())
            {
                throw new ControllerException("not found users", HttpStatusCode.NotFound);
            }

            Log.Information("Retrieved {Count} users successfully", users.Count());
            return Results.Ok(users);
        }

        [HttpGet("getUser")]
        public IResult GetUser(Guid id)
        {
            var user = _userService.Get(id);
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
        public async Task<IResult> UpdateUser(User user)
        {
            var result = await _userService.Update(user);
            if (!result)
            {
                throw new ControllerException("bad update data", HttpStatusCode.BadRequest);
            }
            Log.Information("Successfully updated user with ID {UserId}", user.Id);
            return Results.Ok(user);
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
            return Results.Ok(cart);
        }

        [HttpGet("getLastOrder")]
        [Authorize]
        public async Task<IResult> GetLastOrder(Guid userId)
        {
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
            return Results.Ok(lastOrder);
        }
    }
}
